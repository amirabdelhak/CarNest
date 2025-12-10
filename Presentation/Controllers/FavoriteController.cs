using BLL.Manager.FavoriteManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.Requests;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteManager manager;

        public FavoriteController(IFavoriteManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        public IActionResult GetMyFavorites()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(manager.GetCustomerFavorites(customerId));
        }

        [HttpPost]
        public IActionResult AddToFavorites(FavoriteRequest request)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var result = manager.AddToFavorites(customerId, request.CarId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{carId}")]
        public IActionResult RemoveFromFavorites(string carId)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            manager.RemoveFromFavorites(customerId, carId);
            return Ok(new { Message = "Removed from favorites" });
        }
    }
}
