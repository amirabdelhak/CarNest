using BLL.Manager.ModelManager;
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
    public class ModelController : ControllerBase
    {
        private readonly IModelManager manager;

        public ModelController(IModelManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await manager.GetAllAsync());

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id) => Ok(await manager.GetByIdAsync(id));

        // Get models filtered by make ID. Used for dynamic dropdown filtering.
        [HttpGet("by-make/{makeId:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByMakeId(int makeId) => Ok(await manager.GetByMakeIdAsync(makeId));

        [HttpPost]
        public async Task<IActionResult> Add(ModelRequest request)
        {
            var result = await manager.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.ModelId }, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Model model)
        {
            var result = await manager.UpdateAsync(model);
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
