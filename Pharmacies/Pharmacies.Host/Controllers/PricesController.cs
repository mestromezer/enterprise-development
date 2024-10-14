using Microsoft.AspNetCore.Mvc;
using Pharmacies.Application.Dto;
using Pharmacies.Application.Interfaces;

namespace Pharmacies.Controllers;

/// <summary>
/// CRUD for prices
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PricesController(IEntityService<PriceDto, int> priceService) : ControllerBase
{
    /// <summary>
    /// Read
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PriceDto>>> GetPrices()
    {
        var prices = await priceService.GetAll();
        return Ok(prices);
    }

    /// <summary>
    /// Read
    /// </summary>
    /// <param name="id">Price ID</param>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PriceDto>> GetPrice(int id)
    {
        var price = await priceService.GetByKey(id);
        if (price == null)
        {
            return NotFound();
        }

        return Ok(price);
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="priceDto">Data for new price</param>
    [HttpPost]
    public async Task<ActionResult<PriceDto>> CreatePrice(PriceDto priceDto)
    {
        await priceService.Add(priceDto);
        return CreatedAtAction(nameof(GetPrice), new { id = priceDto.Id }, priceDto);
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="id">Price ID</param>
    /// <param name="priceDto">Updated price data</param>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePrice(int id, PriceDto priceDto)
    {
        var existingPrice = await priceService.GetByKey(id);
        if (existingPrice == null)
        {
            return NotFound();
        }

        await priceService.Update(id, priceDto);
        return NoContent();
    }

    /// <summary>
    /// Delete
    /// </summary>
    /// <param name="id">Price ID</param>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePrice(int id)
    {
        var price = await priceService.GetByKey(id);
        if (price == null)
        {
            return NotFound();
        }

        await priceService.Delete(id);
        return NoContent();
    }
}
