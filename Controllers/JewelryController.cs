using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;
using WebApi.Models;
using System.Linq;
using WebApi.Services;

namespace WebApi.controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JewelryController : ControllerBase
    {
        private IJewelryService jewelryService;
        public JewelryController(IJewelryService jewelryService)
        {
            this.jewelryService = jewelryService;
        }

        [HttpGet]
        public ActionResult<List<Jewelry>> GetAll() =>
            jewelryService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Jewelry> Get(int id)
        {
            var jewelry = jewelryService.Get(id);
            if (jewelry == null)
                return NotFound();
            return jewelry;
        }

        [HttpPost]
        public IActionResult Create(Jewelry newJewelry)
        {
            jewelryService.Add(newJewelry);
            return CreatedAtAction(nameof(Create), new { id = newJewelry.Id }, newJewelry);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, Jewelry newJewelry)
        {
            if (id != newJewelry.Id)
                return BadRequest();
            var existingJewelry = jewelryService.Get(id);
            if (existingJewelry is null)
                return NotFound();
            jewelryService.Update(newJewelry);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var jewelry = jewelryService.Get(id);
            if (jewelry is null)
                return NotFound();

            jewelryService.Delete(id);

            return Content(jewelryService.Count.ToString());
        }
    }
}