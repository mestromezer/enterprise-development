using Microsoft.AspNetCore.Mvc;
using Pharmacies.Interfaces;
using Pharmacies.Model;

namespace Pharmacies.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PricesController(IRepository<Price, int> repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Price>>> GetPrices()
    {
        var prices = await repository.GetAsList();
        return Ok(prices);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Price>> GetPrice(int id)
    {
        var price = (await repository.GetAsList(p => p.Id == id)).FirstOrDefault();
        if (price == null)
        {
            return NotFound();
        }

        return Ok(price);
    }

    [HttpPost]
    public async Task<ActionResult<Price>> CreatePrice(Price price)
    {
        await repository.Add(price);
        return CreatedAtAction(nameof(GetPrice), new { id = price.Id }, price);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePrice(int id, Price updatedPrice)
    {
        if (id != updatedPrice.Id)
        {
            return BadRequest("Price ID mismatch.");
        }

        var price = (await repository.GetAsList(p => p.Id == id)).FirstOrDefault();
        if (price is null)
        {
            return NotFound();
        }

        await repository.Update(updatedPrice);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePrice(int id)
    {
        var price = (await repository.GetAsList(p => p.Id == id)).FirstOrDefault();
        if (price is null)
        {
            return NotFound();
        }

        await repository.Delete(id);
        return NoContent();
    }
}