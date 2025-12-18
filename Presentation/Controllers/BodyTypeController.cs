using BLL.Manager.BodyTypeManager;
using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Configuration;
using Presentation.DTOs.Requests;
using Presentation.DTOs.Responses;

using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BodyTypeController : ControllerBase
    {
        private readonly IBodyTypeManager manager;

        public BodyTypeController(IBodyTypeManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await manager.GetAllAsync());
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await manager.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BodyTypeRequest request)
        {
            var result = await manager.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.BodyId }, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(BodyType bodyType)
        {
            var result = await manager.UpdateAsync(bodyType);
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
