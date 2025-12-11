using BLL.Manager.ModelManager;
using DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.Requests;

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
        public IActionResult GetAll() => Ok(manager.GetAll());

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public IActionResult GetById(int id) => Ok(manager.GetById(id));

        [HttpPost]
        public IActionResult Add(ModelRequest request)
        {
            var result = manager.Add(request);
            return CreatedAtAction(nameof(GetById), new { id = result.ModelId }, result);
        }

        [HttpPut]
        public IActionResult Update(Model model)
        {
            var result = manager.Update(model);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            manager.Delete(id);
            return Ok();
        }
    }
}
