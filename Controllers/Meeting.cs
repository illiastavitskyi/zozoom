using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZoZoom.Controllers
{
    public class Meeting : Controller
    {
        [Authorize]
        public IActionResult Index(string room = "ZoZoomGeneral")
        {
            ViewBag.JitsiToken = Environment.GetEnvironmentVariable("JITSI_JWT_TOKEN") ?? "eyJraWQiOiJ2cGFhcy1tYWdpYy1jb29raWUtNmYzOGZjMDQyYjAxNDY0N2ExNGRhNmMyMzRiN2ExMTYvMTcwNzAxLVNBTVBMRV9BUFAiLCJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiJqaXRzaSIsImlzcyI6ImNoYXQiLCJpYXQiOjE3ODA0MTUzOTYsImV4cCI6MTc4MDQyMjU5NiwibmJmIjoxNzgwNDE1MzkxLCJzdWIiOiJ2cGFhcy1tYWdpYy1jb29raWUtNmYzOGZjMDQyYjAxNDY0N2ExNGRhNmMyMzRiN2ExMTYiLCJjb250ZXh0Ijp7ImZlYXR1cmVzIjp7ImxpdmVzdHJlYW1pbmciOnRydWUsImZpbGUtdXBsb2FkIjp0cnVlLCJvdXRib3VuZC1jYWxsIjp0cnVlLCJzaXAtb3V0Ym91bmQtY2FsbCI6ZmFsc2UsInRyYW5zY3JpcHRpb24iOnRydWUsImxpc3QtdmlzaXRvcnMiOmZhbHNlLCJyZWNvcmRpbmciOnRydWUsImZsaXAiOmZhbHNlfSwidXNlciI6eyJoaWRkZW4tZnJvbS1yZWNvcmRlciI6ZmFsc2UsIm1vZGVyYXRvciI6dHJ1ZSwibmFtZSI6ImJhc2htYWNrMDA3IiwiaWQiOiJnb29nbGUtb2F1dGgyfDExNDQ2MDE3NDAxMzM5MDg2MzYwOSIsImF2YXRhciI6IiIsImVtYWlsIjoiYmFzaG1hY2swMDdAZ21haWwuY29tIn19LCJyb29tIjoiKiJ9.UH0PWlmQ7iHxsW0o_3-GOHi_JnMt-dqnTKOprS193kJrN8hhDCfCRpaLvE0aaTdBzS6qxuQRdz6QDviOIazyfOmoAWGL3ojZlqQKIyftMuKIVpf6uKzu1wXg_6_ikHoFEX2R9yM1BjJcBPgzj3hWws98JUT28DWyp6eTEHjkrdVf4-8FFx9cKlDaKTvEv1Br1rQqmlWOfnTm7PM6EikLx7I0HfRlwJes9Yd0rKKVAqYvkH62TSaD8mcB1iDuzo0qJs5O0O4MGdsG52yBOZ2opwBS0a_V4ClCcxkAODftXjqobdykEMEccLS3tzd4_KbM98sPq8Gdok7dFArH5uz_Ng";
            ViewBag.JitsiAppId = Environment.GetEnvironmentVariable("JITSI_APP_ID") ?? "vpaas-magic-cookie-6f38fc042b014647a14da6c234b7a116";
            ViewBag.RoomName = room;

            return View();
        }
    }
}
