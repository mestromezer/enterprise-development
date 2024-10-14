using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pharmacies.Application.Dto;
using Pharmacies.Application.Interfaces;

namespace Pharmacies.Controllers;

/// <summary>
/// CRUD Pharmaceutical Groups
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PharmaceuticalGroupsController(IEntityService<PharmaceuticalGroupDto, int> pharmaceuticalGroupService, IMapper mapper)
    : ControllerBase
{
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Read
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PharmaceuticalGroupDto>>> GetPharmaceuticalGroups()
    {
        var groups = await pharmaceuticalGroupService.GetAll();
        return Ok(groups);
    }

    /// <summary>
    /// Read
    /// </summary>
    /// <param name="id">Key</param>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PharmaceuticalGroupDto>> GetPharmaceuticalGroup(int id)
    {
        var group = await pharmaceuticalGroupService.GetByKey(id);
        if (group == null)
        {
            return NotFound();
        }

        return Ok(group);
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="groupDto">Data</param>
    [HttpPost]
    public async Task<ActionResult<PharmaceuticalGroupDto>> CreatePharmaceuticalGroup(PharmaceuticalGroupDto groupDto)
    {
        await pharmaceuticalGroupService.Add(groupDto);
        return CreatedAtAction(nameof(GetPharmaceuticalGroup), new { id = groupDto.Id }, groupDto);
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="id">Key</param>
    /// <param name="updatedGroupDto">Updated Data</param>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePharmaceuticalGroup(int id, PharmaceuticalGroupDto updatedGroupDto)
    {
        await pharmaceuticalGroupService.Update(id, updatedGroupDto);
        return NoContent();
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="id">Key</param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePharmaceuticalGroup(int id)
    {
        await pharmaceuticalGroupService.Delete(id);
        return NoContent();
    }
}