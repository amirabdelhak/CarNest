using Azure.Core;
using BLL.Manager.FuelTypeManager;
using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.Requests;

using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class FuelTypeController : ControllerBase
    {
        private readonly IFuelTypeManager manager;

        public FuelTypeController(IFuelTypeManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await manager.GetAllAsync());

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await manager.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(FuelTypeRequest request)
        {
            var result = await manager.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.FuelId }, result);

        }
        [HttpPut]
        public async Task<IActionResult> Update(FuelType fuelType)
        {
            var result = await manager.UpdateAsync(fuelType);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await manager.DeleteAsync(id);
            return Ok();
        }
    }
}
