using Microsoft.AspNetCore.Mvc;
using Pharmacies.Interfaces;
using Pharmacies.Model.Reference;

namespace Pharmacies.Controllers;

[Route("api/[controller]/{dictionaryType}")]
[ApiController]
public class DictionariesController(
    IRepository<ProductGroup, int> productGroupRepository,
    IRepository<PharmaceuticalGroup, int> pharmaceuticalGroupRepository)
    : ControllerBase
{
    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<object>>> GetDictionaryItems(string dictionaryType)
    {
        return dictionaryType.ToLower() switch
        {
            "product_group" => Ok(await productGroupRepository.GetAsList()),
            "pharmaceutical_group" => Ok(await pharmaceuticalGroupRepository.GetAsList()),
            _ => BadRequest("Invalid dictionary type.")
        };
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<object>> GetDictionaryItem(string dictionaryType, int id)
    {
        switch (dictionaryType.ToLower())
        {
            case "product_group":
                var productGroup = (await productGroupRepository.GetAsList(pg => pg.Id == id)).FirstOrDefault();
                if (productGroup == null) return NotFound();
                return Ok(productGroup);

            case "pharmaceutical_group":
                var pharmaceuticalGroup =
                    (await pharmaceuticalGroupRepository.GetAsList(pg => pg.Id == id)).FirstOrDefault();
                if (pharmaceuticalGroup == null) return NotFound();
                return Ok(pharmaceuticalGroup);

            default:
                return BadRequest("Invalid dictionary type.");
        }
    }

    [HttpPost("")]
    public async Task<ActionResult<object>> CreateDictionaryItem(string dictionaryType, object newItem)
    {
        switch (dictionaryType.ToLower())
        {
            case "product_group":
                if (newItem is ProductGroup productGroup)
                {
                    await productGroupRepository.Add(productGroup);
                    return CreatedAtAction(nameof(GetDictionaryItem),
                        new { dictionaryType = "product_group", id = productGroup.Id }, productGroup);
                }

                break;

            case "pharmaceutical_group":
                if (newItem is PharmaceuticalGroup pharmaceuticalGroup)
                {
                    await pharmaceuticalGroupRepository.Add(pharmaceuticalGroup);
                    return CreatedAtAction(nameof(GetDictionaryItem),
                        new { dictionaryType = "pharmaceutical_group", id = pharmaceuticalGroup.Id },
                        pharmaceuticalGroup);
                }

                break;
        }

        return BadRequest("Invalid dictionary type or item.");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDictionaryItem(string dictionaryType, int id, object updatedItem)
    {
        switch (dictionaryType.ToLower())
        {
            case "product_group":
                if (updatedItem is ProductGroup productGroup && id == productGroup.Id)
                {
                    await productGroupRepository.Update(productGroup);
                    return NoContent();
                }

                break;

            case "pharmaceutical_group":
                if (updatedItem is PharmaceuticalGroup pharmaceuticalGroup && id == pharmaceuticalGroup.Id)
                {
                    await pharmaceuticalGroupRepository.Update(pharmaceuticalGroup);
                    return NoContent();
                }

                break;
        }

        return BadRequest("Invalid dictionary type or item.");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDictionaryItem(string dictionaryType, int id)
    {
        switch (dictionaryType.ToLower())
        {
            case "product_group":
                await productGroupRepository.Delete(id);
                return NoContent();

            case "pharmaceutical_group":
                await pharmaceuticalGroupRepository.Delete(id);
                return NoContent();

            default:
                return BadRequest("Invalid dictionary type.");
        }
    }
}