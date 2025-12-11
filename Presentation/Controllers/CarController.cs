using System.Security.Claims;
using BLL.Manager.CarManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.Requests;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CarController : ControllerBase
    {
        private readonly ICarManager manager;

        public CarController(ICarManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// Get all cars. Vendors see only their cars, others see all cars.
        /// </summary>
        [HttpGet]
        public IActionResult GetAll([FromQuery] PaginationRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            // If Vendor, return only their cars
            if (userRole == "Vendor" && !string.IsNullOrEmpty(userId))
            {
                return Ok(manager.GetCarsByVendor(request, userId));
            }

            // Otherwise return all cars (Admin, Customer, or anonymous)
            return Ok(manager.GetAll(request));
        }
        /// <summary>
        /// Get car details by ID. Accessible to everyone.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var result = manager.GetById(id);
            if (result == null) return NotFound(new { Message = "Car not found" });
            return Ok(result);
        }

        /// <summary>
        /// Add a new car with images. Only Admin and Vendor can add cars.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Vendor")]
        public IActionResult Add([FromForm] CarRequest request, [FromForm] IFormFileCollection? images)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User ID not found in token" });
            }

            try
            {
                // Ownership Model: Either Admin OR Vendor owns the car
                string? adminId = userRole == "Admin" ? userId : null;
                string? vendorId = userRole == "Vendor" ? userId : null;

                var result = manager.Add(request, adminId, vendorId, images);
                return CreatedAtAction(nameof(GetById), new { id = result.CarId }, result);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException != null ? $"{ex.Message} | Inner: {ex.InnerException.Message}" : ex.Message;
                return BadRequest(new { Message = message });
            }
        }

        /// <summary>
        /// Update an existing car. Admin can update any car, Vendor can only update their own cars.
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Vendor")]
        public IActionResult Update(
            string id,
            [FromForm] CarRequest request,
            [FromForm] IFormFileCollection? newImages,
            [FromForm] string? imagesToDeleteJson)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            try
            {
                // Parse images to delete from JSON
                List<string>? imagesToDelete = null;
                if (!string.IsNullOrEmpty(imagesToDeleteJson))
                {
                    imagesToDelete = System.Text.Json.JsonSerializer.Deserialize<List<string>>(imagesToDeleteJson);
                }

                var result = manager.Update(id, request, userId, userRole, newImages, imagesToDelete);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Delete a car and all its images. Admin can delete any car, Vendor can only delete their own cars.
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Vendor")]
        public IActionResult Delete(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            try
            {
                manager.Delete(id, userId, userRole);
                return Ok(new { Message = "Car and associated images deleted successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
