using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ZoZoom.Models;

namespace ZoZoom.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.JitsiToken = Environment.GetEnvironmentVariable("JITSI_JWT_TOKEN") ?? "eyJraWQiOiJ2cGFhcy1tYWdpYy1jb29raWUtNmYzOGZjMDQyYjAxNDY0N2ExNGRhNmMyMzRiN2ExMTYvMTcwNzAxLVNBTVBMRV9BUFAiLCJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiJqaXRzaSIsImlzcyI6ImNoYXQiLCJpYXQiOjE3ODAzMzYyOTAsImV4cCI6MTc4MDM0MzQ5MCwibmJmIjoxNzgwMzM2Mjg1LCJzdWIiOiJ2cGFhcy1tYWdpYy1jb29raWUtNmYzOGZjMDQyYjAxNDY0N2ExNGRhNmMyMzRiN2ExMTYiLCJjb250ZXh0Ijp7ImZlYXR1cmVzIjp7ImxpdmVzdHJlYW1pbmciOnRydWUsImZpbGUtdXBsb2FkIjp0cnVlLCJvdXRib3VuZC1jYWxsIjp0cnVlLCJzaXAtb3V0Ym91bmQtY2FsbCI6ZmFsc2UsInRyYW5zY3JpcHRpb24iOnRydWUsImxpc3QtdmlzaXRvcnMiOmZhbHNlLCJyZWNvcmRpbmciOnRydWUsImZsaXAiOmZhbHNlfSwidXNlciI6eyJoaWRkZW4tZnJvbS1yZWNvcmRlciI6ZmFsc2UsIm1vZGVyYXRvciI6dHJ1ZSwibmFtZSI6ImJhc2htYWNrMDA3IiwiaWQiOiJnb29nbGUtb2F1dGgyfDExNDQ2MDE3NDAxMzM5MDg2MzYwOSIsImF2YXRhciI6IiIsImVtYWlsIjoiYmFzaG1hY2swMDdAZ21haWwuY29tIn19LCJyb29tIjoiKiJ9.uB3KflhSzZnkN0Xl9s7tQTxxpLqBaTSdNFzaT-6_MrtoNYJJ-Nm8owAcsclJnCG3zPME-eIx-w9ekdEmh6IZtLuXY2sEWMx8jtOJxRBlgyzbDxbhDi6_bYWh0UgPNEMNGU-bdLFIOQpQHeI4wLAnL3Kf3VF5dgXBC90hgTUEXnG1BeeECcXUvWFh8ag6iuVoT0bpsg1BDQ6Io7seOUc6XquRXqRYbu0CEnclK4QD36jgvoGFbbP2saMeEMouGhNg-JgShhG71qv9lyUxrKngJsXurciP9PoJVzzwpmDf4G1WSCO3TDhrd5XkHqAzG9zKrL3UBb7cSV3-HuoYlNer2w";
            ViewBag.JitsiAppId = Environment.GetEnvironmentVariable("JITSI_APP_ID") ?? "vpaas-magic-cookie-6f38fc042b014647a14da6c234b7a116";
            ViewBag.RoomName = "ZoZoomRoom-" + Guid.NewGuid().ToString().Substring(0, 8);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
