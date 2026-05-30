using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZoZoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private const long MaxFileBytes = 500L * 1024L * 1024L; // 500 MB

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        // Allow up to 500 MB for this action
        [RequestSizeLimit(MaxFileBytes)]
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "No file uploaded." });

            if (file.Length > MaxFileBytes)
                return StatusCode(413, new { error = "File too large. Maximum allowed size is 500 MB." });

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString("N") + "_" + Path.GetFileName(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Stream directly to disk (avoids buffering whole file in memory)
            await using (var targetStream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(targetStream);
            }

            var fileUrl = $"/uploads/{uniqueFileName}";
            return Ok(new { url = fileUrl });
        }
    }
}