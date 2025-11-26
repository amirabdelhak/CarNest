using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.Manager.CarImageManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Responses;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarImageController : ControllerBase
    {
        private readonly ICarImageManager carImageManager;
        private readonly IWebHostEnvironment env;

        public CarImageController(ICarImageManager carImageManager, IWebHostEnvironment env)
        {
            this.carImageManager = carImageManager;
            this.env = env;
        }

        [HttpPost("upload/{carId}")]
        public async Task<IActionResult> Upload(string carId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // ensure extension is permitted (basic validation)
            var permitted = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!permitted.Contains(ext))
                return BadRequest("Invalid file type.");

            var uploadsRoot = Path.Combine(env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads", "cars", carId);
            Directory.CreateDirectory(uploadsRoot);

            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(uploadsRoot, fileName);

            await using (var stream = System.IO.File.Create(fullPath))
            {
                await file.CopyToAsync(stream);
            }

            var publicUrl = $"{Request.Scheme}://{Request.Host}/uploads/cars/{carId}/{fileName}";

            var saved = carImageManager.Add(carId, publicUrl);

            var response = new CarImageResponse
            {
                ImageId = saved.ImageId,
                CarId = saved.CarId,
                ImageUrl = saved.ImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = saved.ImageId }, response);
        }

        [HttpGet("car/{carId}")]
        public IActionResult GetByCar(string carId) => Ok(carImageManager.GetByCarId(carId).Select(i => new CarImageResponse { ImageId = i.ImageId, CarId = i.CarId, ImageUrl = i.ImageUrl }));

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var img = carImageManager.GetById(id);
            if (img == null) return NotFound();
            return Ok(new CarImageResponse { ImageId = img.ImageId, CarId = img.CarId, ImageUrl = img.ImageUrl });
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var img = carImageManager.GetById(id);
            if (img == null) return NotFound();

            // optionally delete physical file (best-effort) 
            try
            {
                var uri = new Uri(img.ImageUrl);
                var localPath = uri.AbsolutePath.TrimStart('/'); // "uploads/cars/..."
                var path = Path.Combine(env.WebRootPath ?? "wwwroot", localPath.Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }
            catch { /* ignore on failure */ }

            carImageManager.Delete(id);
            return Ok();
        }
    }
}