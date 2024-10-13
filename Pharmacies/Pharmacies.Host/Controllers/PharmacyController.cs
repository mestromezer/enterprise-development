using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Pharmacies.Application.Dto;
using Pharmacies.Application.Interfaces;

namespace Pharmacies.Controllers;

/// <summary>
/// CRUD Pharmacies
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PharmaciesController(IEntityService<PharmacyDto, int> pharmacyService, IMapper mapper)
    : ControllerBase
{
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Read
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PharmacyDto>>> GetPharmacies()
    {
        var pharmacies = await pharmacyService.GetAll();
        return Ok(pharmacies);
    }
    
    /// <summary>
    /// Read
    /// </summary>
    /// <param name="number">Key</param>
    [HttpGet("{number:int}")]
    public async Task<ActionResult<PharmacyDto>> GetPharmacy(int number)
    {
        var pharmacy = await pharmacyService.GetByKey(number);
        if (pharmacy == null)
        {
            return NotFound();
        }

        return Ok(pharmacy);
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="pharmacyDto">Data</param>
    [HttpPost]
    public async Task<ActionResult<PharmacyDto>> CreatePharmacy(PharmacyDto pharmacyDto)
    {
        await pharmacyService.Add(pharmacyDto);
        return CreatedAtAction(nameof(GetPharmacy), new { number = pharmacyDto.Number }, pharmacyDto);
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="number">Key</param>
    /// <param name="updatedPharmacyDto">Data</param>
    [HttpPut("{number:int}")]
    public async Task<IActionResult> UpdatePharmacy(int number, PharmacyDto updatedPharmacyDto)
    {
        await pharmacyService.Update(number, updatedPharmacyDto);
        return NoContent();
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="number">Key</param>
    [HttpDelete("{number:int}")]
    public async Task<IActionResult> DeletePharmacy(int number)
    {
        await pharmacyService.Delete(number);
        return NoContent();
    }
}
