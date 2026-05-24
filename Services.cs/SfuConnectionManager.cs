using SIPSorcery.Net;
using SIPSorceryMedia.Abstractions;
using System.Collections.Concurrent;

namespace ZoZoom.Services
{
    public class PeerState
    {
        public required RTCPeerConnection Connection { get; set; }
        public uint VideoSSRC { get; set; }
        public uint AudioSSRC { get; set; }
        public HashSet<string> AddedParticipants { get; set; } = new();
    }

    public class SfuConnectionManager
    {
        private readonly ConcurrentDictionary<string, PeerState> _peers = new();

        // Dictionary вместо одного колбека — каждый connectionId имеет свой sender
        private readonly ConcurrentDictionary<string, Func<string, Task>> _senders = new();

        public void RegisterSender(string connectionId, Func<string, Task> sender)
        {
            _senders[connectionId] = sender;
        }

        public void UnregisterSender(string connectionId)
        {
            _senders.TryRemove(connectionId, out _);
        }

        public async Task<string> ProcessSdpOffer(string connectionId, string sdpOffer)
        {
            var peer = new RTCPeerConnection(null);
            var rng = new Random();
            var state = new PeerState
            {
                Connection = peer,
                VideoSSRC = (uint)rng.Next(1000, int.MaxValue),
                AudioSSRC = (uint)rng.Next(1000, int.MaxValue)
            };

            peer.addTrack(new MediaStreamTrack(new VideoFormat(VideoCodecsEnum.VP8, 96)));
            peer.addTrack(new MediaStreamTrack(new AudioFormat(AudioCodecsEnum.PCMU, 0)));

            peer.OnRtpPacketReceived += (ep, mediaType, pkt) =>
            {
                foreach (var (id, otherState) in _peers)
                {
                    if (id == connectionId) continue;
                    if (otherState.Connection.connectionState != RTCPeerConnectionState.connected) continue;

                    otherState.Connection.SendRtpRaw(
                        mediaType,
                        pkt.Payload,
                        pkt.Header.Timestamp,
                        pkt.Header.MarkerBit,
                        pkt.Header.PayloadType);
                }
            };

            peer.onconnectionstatechange += async peerState =>
            {
                Console.WriteLine($"[{connectionId}] → {peerState}");

                if (peerState == RTCPeerConnectionState.connected)
                    await RenegotiateAllExcept(connectionId);

                if (peerState is RTCPeerConnectionState.closed or RTCPeerConnectionState.failed)
                {
                    RemovePeer(connectionId);
                    await RenegotiateAllExcept(connectionId);
                }
            };

            _peers.TryAdd(connectionId, state);

            peer.setRemoteDescription(new RTCSessionDescriptionInit
            {
                type = RTCSdpType.offer,
                sdp = sdpOffer
            });

            var answer = peer.createAnswer(null);
            await peer.setLocalDescription(answer);

            return await WaitForIce(peer);
        }

        public void ProcessSdpAnswer(string connectionId, string sdpAnswer)
        {
            if (!_peers.TryGetValue(connectionId, out var state)) return;

            state.Connection.setRemoteDescription(new RTCSessionDescriptionInit
            {
                type = RTCSdpType.answer,
                sdp = sdpAnswer
            });
        }

        private async Task RenegotiateAllExcept(string excludeId)
        {
            foreach (var (id, state) in _peers)
            {
                if (id == excludeId) continue;
                if (state.Connection.connectionState != RTCPeerConnectionState.connected) continue;
                if (!_senders.ContainsKey(id)) continue;

                try
                {
                    // Добавляем треки новых участников
                    foreach (var (otherId, _) in _peers)
                    {
                        if (otherId == id) continue;
                        if (state.AddedParticipants.Contains(otherId)) continue;

                        state.Connection.addTrack(new MediaStreamTrack(
                            new VideoFormat(VideoCodecsEnum.VP8, 96)));
                        state.Connection.addTrack(new MediaStreamTrack(
                            new AudioFormat(AudioCodecsEnum.PCMU, 0)));
                        state.AddedParticipants.Add(otherId);
                    }

                    var offer = state.Connection.createOffer(null);
                    await state.Connection.setLocalDescription(offer);

                    // Снимаем старый handler перед добавлением нового
                    var sdp = await WaitForIce(state.Connection);

                    await _senders[id](sdp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Renegotiation failed for {id}: {ex.Message}");
                }
            }
        }

        // Отдельный метод для ICE gathering — без накопления handlers
        private static async Task<string> WaitForIce(RTCPeerConnection peer)
        {
            var tcs = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            Action<RTCIceCandidate>? handler = null;
            handler = candidate =>
            {
                if (candidate == null)
                {
                    peer.onicecandidate -= handler;
                    tcs.TrySetResult(peer.localDescription.sdp.ToString());
                }
            };
            peer.onicecandidate += handler;

            var winner = await Task.WhenAny(tcs.Task, Task.Delay(5000));
            return winner == tcs.Task ? await tcs.Task : peer.localDescription.sdp.ToString();
        }

        public void RemovePeer(string connectionId)
        {
            if (_peers.TryRemove(connectionId, out var state))
                state.Connection.Close("disconnected");
        }
    }
}