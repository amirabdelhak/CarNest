using BLL.Manager.FavoriteManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.Requests;
using System.Security.Claims;

using System.Threading.Tasks;

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
        public async Task<IActionResult> GetMyFavorites()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await manager.GetCustomerFavoritesAsync(customerId));
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites(FavoriteRequest request)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var result = await manager.AddToFavoritesAsync(customerId, request.CarId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{carId}")]
        public async Task<IActionResult> RemoveFromFavorites(string carId)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await manager.RemoveFromFavoritesAsync(customerId, carId);
            return Ok(new { Message = "Removed from favorites" });
        }
    }
}
