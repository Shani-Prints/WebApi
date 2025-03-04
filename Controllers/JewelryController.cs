
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Models;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = "User")]
    public class JewelryController : ControllerBase
    {
        private IJewelryService jewelryService;
        private int? UserId
        {
            get
            {
                var idClaim = User.FindFirst("id");
                return idClaim != null && int.TryParse(idClaim.Value, out int userId) ? userId : (int?)null;
            }
        }
        private string? userRole
        {
            get
            {
                var roleClaim = User.FindFirst("type");
                return roleClaim?.Value;
            }
        }
        
        [HttpGet]
        public ActionResult<List<Jewelry>> GetAll()
        {
            var jewelryList = jewelryService.GetAll() ?? new List<Jewelry>();

            if (userRole == "Admin")
                return jewelryList;

            return jewelryList.Where(j => j.UserId == UserId).ToList();
        }

        public JewelryController(IJewelryService jewelryService)
        {
            this.jewelryService = jewelryService;
        }

        [HttpGet("{id}")]
        public ActionResult<Jewelry> Get(int id)
        {
            var Jewelry = jewelryService.Get(id);
            if (Jewelry?.UserId != UserId)
                return Unauthorized();

            if (Jewelry == null)
                return NotFound();
            return Jewelry;
        }

        [HttpPost]
        public IActionResult Create(Jewelry jewelry)
        {
            jewelryService.Add(jewelry, UserId.GetValueOrDefault());
            return CreatedAtAction(nameof(Create), new { id = jewelry.Id }, jewelry);
        }


        [HttpPut("{id}")]
        public ActionResult Update(int id, Jewelry jewelry)
        {
            if (userRole != "Admin" && id != jewelry.Id)
                return Unauthorized();
            var exitingJewelry = jewelryService.Get(id);
            if (exitingJewelry is null)
                return NotFound();
            if (userRole != "Admin" && UserId != exitingJewelry.UserId)
                return Unauthorized();
            jewelryService.Update(jewelry, UserId.GetValueOrDefault());
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var jewelry = jewelryService.Get(id);
            if (jewelry is null)
                return NotFound();

            if (jewelry.UserId != UserId && userRole != "Admin")
                return Unauthorized();

            jewelryService.Delete(id);
            return Content(jewelryService.Count.ToString());

        }
    }
}