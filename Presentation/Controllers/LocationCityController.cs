using Azure.Core;
using BLL.Manager.LocationManager;
using DAL.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs.Requests;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationCityController : ControllerBase
    {
        private readonly ILocationManager manager;

        public LocationCityController(ILocationManager manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(manager.GetAll());

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var result = manager.GetById(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Add(LocationCityRequest request)
        {
            var result = manager.Add(request);
            return CreatedAtAction(nameof(GetById), new { id = result.LocId }, result);
        }

        [HttpPut]
        public IActionResult Update(LocationCity location)
        {
            var result = manager.Update(location);
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
