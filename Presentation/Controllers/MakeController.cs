using BLL.Manager.MakeManager;
using DAL.Entity;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.Requests;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MakeController : ControllerBase
    {
        private readonly IMakeManager manager;

        public MakeController(IMakeManager manager)
        {
            this.manager = manager;
        }
        [HttpGet]
        public IActionResult GetAll() => Ok(manager.GetAll());

        [HttpGet("{id:int}")]
        public IActionResult GetById() => Ok(manager.GetAll());

        [HttpPost]
        public IActionResult Add(MakeRequest request)
        {
            var result = manager.Add(request);
            return CreatedAtAction(nameof(GetById), new { id = result.MakeId }, result);
        }

        [HttpPut]
        public IActionResult Update(Make make)
        {
            var result = manager.Update(make);
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
