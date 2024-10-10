using Microsoft.AspNetCore.Mvc;
using Pharmacies.Interfaces;
using Pharmacies.Model;

namespace Pharmacies.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PositionsController(IRepository<Position, int> repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Position>>> GetPositions()
    {
        var positions = await repository.GetAsList();
        return Ok(positions);
    }

    [HttpGet("{code:int}")]
    public async Task<ActionResult<Position>> GetPosition(int code)
    {
        var position = (await repository.GetAsList(p => p.Code == code)).FirstOrDefault();
        if (position == null)
        {
            return NotFound();
        }

        return Ok(position);
    }

    [HttpPost]
    public async Task<ActionResult<Position>> CreatePosition(Position position)
    {
        await repository.Add(position);
        return CreatedAtAction(nameof(GetPosition), new { code = position.Code }, position);
    }

    [HttpPut("{code:int}")]
    public async Task<IActionResult> UpdatePosition(int code, Position updatedPosition)
    {
        if (code != updatedPosition.Code)
        {
            return BadRequest("Position code mismatch.");
        }

        var position = (await repository.GetAsList(p => p.Code == code)).FirstOrDefault();
        if (position is null)
        {
            return NotFound();
        }

        await repository.Update(updatedPosition);
        return NoContent();
    }

    [HttpDelete("{code:int}")]
    public async Task<IActionResult> DeletePosition(int code)
    {
        var position = (await repository.GetAsList(p => p.Code == code)).FirstOrDefault();
        if (position is null)
        {
            return NotFound();
        }

        await repository.Delete(code);
        return NoContent();
    }
}