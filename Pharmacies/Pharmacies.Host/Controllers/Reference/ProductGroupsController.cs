using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pharmacies.Application.Dto.Reference;
using Pharmacies.Application.Interfaces;

namespace Pharmacies.Controllers.Reference;

/// <summary>
/// CRUD Product Groups
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ProductGroupsController(IEntityService<ProductGroupDto, int> productGroupService, IMapper mapper)
    : ControllerBase
{
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Read
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductGroupDto>>> GetProductGroups()
    {
        var groups = await productGroupService.GetAsList();
        return Ok(groups);
    }

    /// <summary>
    /// Read
    /// </summary>
    /// <param name="id">Key</param>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductGroupDto>> GetProductGroup(int id)
    {
        var group = await productGroupService.GetByKey(id);
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
    public async Task<ActionResult<ProductGroupDto>> CreateProductGroup(ProductGroupDto groupDto)
    {
        await productGroupService.Add(groupDto);
        return CreatedAtAction(nameof(GetProductGroup), new { id = groupDto.Id }, groupDto);
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="id">Key</param>
    /// <param name="updatedGroupDto">Updated Data</param>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProductGroup(int id, ProductGroupDto updatedGroupDto)
    {
        await productGroupService.Update(id, updatedGroupDto);
        return NoContent();
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="id">Key</param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProductGroup(int id)
    {
        await productGroupService.Delete(id);
        return NoContent();
    }
}