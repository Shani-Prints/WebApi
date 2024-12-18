using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.controllers;

[ApiController]
[Route("[controller]")]
public class JewelryController : ControllerBase
{
 private static List<Jewelry> list;
    static JewelryController()
    {
        list = new List<Jewelry>
        {
            new Jewelry { Id = 1, Name = "Pearl necklace",Price=1500,Category="necklace" },
            new Jewelry { Id = 2, Name = "Pandora bracelet",Price=350,Category="bracelet" },
            new Jewelry { Id = 3, Name = "Gold watch",Price=6000,Category="watch" },
            
        };
    }

    [HttpGet]
    public IEnumerable<Jewelry> Get()
    {
        return list;
    }
    [HttpGet("{id}")]
    public ActionResult<Jewelry> Get(int id)
    {
        var Jewelry = list.FirstOrDefault(p => p.Id == id);
        if (Jewelry == null)
            return BadRequest("invalid id");
        return Jewelry;
    }

    [HttpPost]
    public ActionResult Insert(Jewelry newJewelry)
    {
        var maxId = list.Max(p => p.Id);
        newJewelry.Id = maxId + 1;
        list.Add(newJewelry);

        return CreatedAtAction(nameof(Insert), new { id = newJewelry.Id }, newJewelry);
    }


    [HttpPut("{id}")]
    public ActionResult Update(int id, Jewelry newJewelry)
    {
        var oldJewelry = list.FirstOrDefault(p => p.Id == id);
        if (oldJewelry == null)
            return BadRequest("invalid id");
        if (newJewelry.Id != newJewelry.Id)
            return BadRequest("id mismatch");

        newJewelry.Name = newJewelry.Name;
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult Remove(int id){
        var oldJewelry = list.FirstOrDefault(p => p.Id == id);
         if (oldJewelry == null)
            return BadRequest("invalid id");
        list.Remove(oldJewelry);    
         return NoContent();
    }
}
