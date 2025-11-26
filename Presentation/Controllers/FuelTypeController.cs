using Azure.Core;
using BLL.Manager.FuelTypeManager;
using DAL.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.Requests;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuelTypeController : ControllerBase
    {
        private readonly IFuelTypeManager manager;

        public FuelTypeController(IFuelTypeManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(manager.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = manager.GetById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Add(FuelTypeRequest request)
        {
            var result = manager.Add(request);
            return CreatedAtAction(nameof(GetById), new { id = result.FuelId }, result);

        }
        [HttpPut]
        public IActionResult Update(FuelType fuelType)
        {
            var result = manager.Update(fuelType);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            manager.Delete(id);
            return Ok();
        }
    }
}
