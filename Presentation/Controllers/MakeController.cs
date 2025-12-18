using BLL.Manager.MakeManager;
using DAL.Entity;
using DAL.UnitOfWork;
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
    public class MakeController : ControllerBase
    {
        private readonly IMakeManager manager;

        public MakeController(IMakeManager manager)
        {
            this.manager = manager;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() => Ok(await manager.GetAllAsync());

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id) => Ok(await manager.GetByIdAsync(id));

        [HttpPost]
        public async Task<IActionResult> Add(MakeRequest request)
        {
            var result = await manager.AddAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.MakeId }, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(Make make)
        {
            var result = await manager.UpdateAsync(make);
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
