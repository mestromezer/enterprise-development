using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pharmacies.Application.Dto;
using Pharmacies.Application.Interfaces;

namespace Pharmacies.Controllers;

/// <summary>
/// CRUD Pharmaceutical Group References
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PharmaceuticalGroupReferencesController(IEntityService<PharmaceuticalGroupReferenceDto, int> pharmaceuticalGroupReferenceService)
    : ControllerBase
{
    /// <summary>
    /// Read
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PharmaceuticalGroupReferenceDto>>> GetPharmaceuticalGroupReferences() =>
        Ok(await pharmaceuticalGroupReferenceService.GetAll());

    /// <summary>
    /// Read
    /// </summary>
    /// <param name="id">id</param>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PharmaceuticalGroupReferenceDto>> GetPharmaceuticalGroupReference(int id)
    {
        var reference = await pharmaceuticalGroupReferenceService.GetByKey(id);
        if (reference == null)
        {
            return NotFound();
        }

        return Ok(reference);
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="referenceDto">Data</param>
    [HttpPost]
    public async Task<ActionResult<PharmaceuticalGroupReferenceDto>> CreatePharmaceuticalGroupReference(PharmaceuticalGroupReferenceDto referenceDto)
    {
        await pharmaceuticalGroupReferenceService.Add(referenceDto);
        return Created();
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="updatedReferenceDto">Data</param>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePharmaceuticalGroupReference(int id, PharmaceuticalGroupReferenceDto updatedReferenceDto)
    {
        await pharmaceuticalGroupReferenceService.Update(id, updatedReferenceDto);
        return NoContent();
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="id">Id</param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePharmaceuticalGroupReference(int id)
    {
        await pharmaceuticalGroupReferenceService.Delete(id);
        return NoContent();
    }
}