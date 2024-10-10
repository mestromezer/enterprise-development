using Microsoft.AspNetCore.Mvc;
using Pharmacies.Interfaces;
using Pharmacies.Model;

namespace Pharmacies.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PharmaciesController(IRepository<Pharmacy, int> repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pharmacy>>> GetPharmacies()
    {
        var pharmacies = await repository.GetAsList();
        return Ok(pharmacies);
    }
    
    [HttpGet("{number:int}")]
    public async Task<ActionResult<Pharmacy>> GetPharmacy(int number)
    {
        var pharmacy = (await repository.GetAsList(p => p.Number == number)).FirstOrDefault();
        if (pharmacy == null)
        {
            return NotFound();
        }

        return Ok(pharmacy);
    }

    [HttpPost]
    public async Task<ActionResult<Pharmacy>> CreatePharmacy(Pharmacy pharmacy)
    {
        await repository.Add(pharmacy);
        return CreatedAtAction(nameof(GetPharmacy), new { number = pharmacy.Number }, pharmacy);
    }

    [HttpPut("{number:int}")]
    public async Task<IActionResult> UpdatePharmacy(int number, Pharmacy updatedPharmacy)
    {
        if (number != updatedPharmacy.Number)
        {
            return BadRequest("Pharmacy number mismatch.");
        }

        var pharmacy = (await repository.GetAsList(p => p.Number == number)).FirstOrDefault();
        if (pharmacy is null)
        {
            return NotFound();
        }

        await repository.Update(updatedPharmacy);
        return NoContent();
    }

    [HttpDelete("{number:int}")]
    public async Task<IActionResult> DeletePharmacy(int number)
    {
        var pharmacy = (await repository.GetAsList(p => p.Number == number)).FirstOrDefault();
        if (pharmacy is null)
        {
            return NotFound();
        }

        await repository.Delete(number);
        return NoContent();
    }
}