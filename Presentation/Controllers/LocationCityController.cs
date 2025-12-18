using Azure.Core;
using BLL.Manager.LocationManager;
using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.Requests;

using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class LocationCityController : ControllerBase
    {
        private readonly ILocationManager manager;

        public LocationCityController(ILocationManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await manager.GetAllAsync());

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await manager.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(LocationCityRequest request)
        {
            var result = await manager.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.LocId }, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(LocationCity location)
        {
            var result = await manager.UpdateAsync(location);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await manager.DeleteAsync(id); 
            return Ok();
        }
    }
}
