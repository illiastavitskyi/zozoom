using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ZoZoom.Models;

namespace ZoZoom.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.JitsiToken = Environment.GetEnvironmentVariable("JITSI_JWT_TOKEN") ?? "eyJraWQiOiJ2cGFhcy1tYWdpYy1jb29raWUtNmYzOGZjMDQyYjAxNDY0N2ExNGRhNmMyMzRiN2ExMTYvMTcwNzAxLVNBTVBMRV9BUFAiLCJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiJqaXRzaSIsImlzcyI6ImNoYXQiLCJpYXQiOjE3ODAxNTEyMDksImV4cCI6MTc4MDE1ODQwOSwibmJmIjoxNzgwMTUxMjA0LCJzdWIiOiJ2cGFhcy1tYWdpYy1jb29raWUtNmYzOGZjMDQyYjAxNDY0N2ExNGRhNmMyMzRiN2ExMTYiLCJjb250ZXh0Ijp7ImZlYXR1cmVzIjp7ImxpdmVzdHJlYW1pbmciOnRydWUsImZpbGUtdXBsb2FkIjp0cnVlLCJvdXRib3VuZC1jYWxsIjp0cnVlLCJzaXAtb3V0Ym91bmQtY2FsbCI6ZmFsc2UsInRyYW5zY3JpcHRpb24iOnRydWUsImxpc3QtdmlzaXRvcnMiOmZhbHNlLCJyZWNvcmRpbmciOnRydWUsImZsaXAiOmZhbHNlfSwidXNlciI6eyJoaWRkZW4tZnJvbS1yZWNvcmRlciI6ZmFsc2UsIm1vZGVyYXRvciI6dHJ1ZSwibmFtZSI6ImJhc2htYWNrMDA3IiwiaWQiOiJnb29nbGUtb2F1dGgyfDExNDQ2MDE3NDAxMzM5MDg2MzYwOSIsImF2YXRhciI6IiIsImVtYWlsIjoiYmFzaG1hY2swMDdAZ21haWwuY29tIn19LCJyb29tIjoiKiJ9.f--yIkQbeiqMwOyu6ymzRGsS0KQMf7OfvvSbP5z3HidV6Y1prAtpaeth8E0MRjt2_orLkP-0JSNT3VJqX0xqJDvfiJ09EcyiOHkiPZ152e-ygocxLzGDxOZ2od2gm199FfXUqmtn1TGAYBEUe6fPRUoAM8t0xB1ATcjAK5apB9mheXcuyvrPt5s0oy3KaZCudM4YCRVkv5jkECVsusowShzMUZGK8M5LbQSJo7r9DoZYnIywefx4oIUSGB9IbyadoxpRsJRhn00p08CfFN6TkgogYK2uiD23TMJm0r1ROvNGUEsIB99zLwBW-0zUhuKmeiYxT8P1xNj-76d3PLMKhg";
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
