using Microsoft.AspNetCore.Mvc;
using Pharmacies.Application.Dto;
using Pharmacies.Application.Interfaces;

namespace Pharmacies.Controllers;

/// <summary>
/// CRUD positions
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PositionsController(IEntityService<PositionDto, int> positionService) : ControllerBase
{
    /// <summary>
    /// Read
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PositionDto>>> GetPositions()
    {
        var positions = await positionService.GetAll();
        return Ok(positions);
    }

    /// <summary>
    /// Read
    /// </summary>
    /// <param name="code">Key</param>
    [HttpGet("{code:int}")]
    public async Task<ActionResult<PositionDto>> GetPosition(int code)
    {
        var position = await positionService.GetByKey(code);
        if (position == null)
        {
            return NotFound();
        }

        return Ok(position);
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="positionDto">Data</param>
    [HttpPost]
    public async Task<ActionResult<PositionDto>> CreatePosition(PositionDto positionDto)
    {
        await positionService.Add(positionDto);
        return CreatedAtAction(nameof(GetPosition), new { code = positionDto.Code }, positionDto);
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="code">Key</param>
    /// <param name="updatedPositionDto">Updated data</param>
    [HttpPut("{code:int}")]
    public async Task<IActionResult> UpdatePosition(int code, PositionDto updatedPositionDto)
    {
        if (code != updatedPositionDto.Code)
        {
            return BadRequest("Position code mismatch.");
        }

        var existingPosition = await positionService.GetByKey(code);
        if (existingPosition == null)
        {
            return NotFound();
        }

        await positionService.Update(code, updatedPositionDto);
        return NoContent();
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="code">Key</param>
    [HttpDelete("{code:int}")]
    public async Task<IActionResult> DeletePosition(int code)
    {
        var position = await positionService.GetByKey(code);
        if (position == null)
        {
            return NotFound();
        }

        await positionService.Delete(code);
        return NoContent();
    }
}
