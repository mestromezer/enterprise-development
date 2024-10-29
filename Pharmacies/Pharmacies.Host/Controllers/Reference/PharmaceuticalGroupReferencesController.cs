using Microsoft.AspNetCore.Mvc;
using Pharmacies.Application.Dto;
using Pharmacies.Application.Dto.Reference;
using Pharmacies.Application.Interfaces;

namespace Pharmacies.Controllers.Reference;

/// <summary>
/// Контроллер работает для установления связей между фарм. группой и позициями в аптеках
/// </summary>
/// <param name="referenceService"></param>
[ApiController]
[Route("api/[controller]")]
public class PharmaceuticalGroupReferenceController(
    IReferenceService<PositionDto, PharmaceuticalGroupDto, int ,int> referenceService)
    : ControllerBase
{
    /// <summary>
    /// Возврашает все связи
    /// </summary>
    /// <returns>Ключ - позиция, значение - список фарм. групп</returns>
    [HttpGet]
    public async Task<ActionResult<IDictionary<PositionDto, IEnumerable<PharmaceuticalGroupDto>>>> GetAllForAll()
    {
        var result = await referenceService.GetAllForAll();
        return Ok(result);
    }

    /// <summary>
    /// По id позиции возвращает список фарм. групп
    /// </summary>
    /// <param name="code">Id позиции</param>
    [HttpGet("{code:int}")]
    public async Task<ActionResult<IEnumerable<PharmaceuticalGroupDto>>> GetFor(int code)
    {
        var result = await referenceService.GetFor(code);

        return Ok(result);
    }

    /// <summary>
    /// Устанавливает связь позиции и фарм. группы
    /// </summary>
    /// <param name="code"></param>
    /// <param name="childKeys"></param>
    /// <returns></returns>
    [HttpPost("{code:int}")]
    public async Task<IActionResult> SetRelation(int code, [FromBody] List<int> childKeys)
    {
        await referenceService.SetRelation(code, childKeys);

        return NoContent();
    }
}