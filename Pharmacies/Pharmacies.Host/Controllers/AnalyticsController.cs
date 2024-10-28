using Microsoft.AspNetCore.Mvc;
using Pharmacies.Application.Dto;
using Pharmacies.Application.Dto.AnalyticsResults;
using Pharmacies.Application.Dto.Reference;
using Pharmacies.Application.Interfaces;

namespace Pharmacies.Controllers;

/// <summary>
/// Контроллер с функционалом аналитических запросов
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AnalyticsController(
    IEntityService<PositionDto, int> positionsService,
    IEntityService<PharmacyDto, int> pharmacyService,
    IEntityService<PriceDto, int> priceService,
    IEntityService<ProductGroupDto, int> productGroupService) : ControllerBase
{
    /// <summary>
    ///  Вывести сведения о всех препаратах в заданной аптеке, упорядочить по названию.
    /// </summary>
    /// <param name="pharmacyNumber">Pharmacy number</param>
    [HttpGet("pharmacy/{pharmacyNumber:int}/positions")]
    public async Task<ActionResult<IEnumerable<PositionDto>>> GetPositionsByPharmacy(int pharmacyNumber)
    {
        var positions = await positionsService.GetAll(p => p.PharmacyId == pharmacyNumber);
    
        var orderedPositions = positions.OrderBy(p => p.Name).ToList();

        if (orderedPositions.Count == 0)
            return NotFound("No drugs found.");

        return Ok(orderedPositions);
    }

    /// <summary>
    /// Вывести для данного препарата подробный список всех аптек с указанием количества препарата в аптеках.
    /// </summary>
    /// <param name="drugName">Drug name</param>
    [HttpGet("drug/{drugName}/pharmacies")]
    public async Task<ActionResult<IEnumerable<PharmacyAndNumericStatisticsDto>>> GetPharmaciesWithDrugQuantity(string drugName)
    {
        var positionsWithDrug = await positionsService.GetAll(p => p.Name == drugName);
        var pharmaciesWithDrug = positionsWithDrug
            .Where(p => p.PharmacyId != null)
            .Select(p => new PharmacyAndNumericStatisticsDto(
                p.PharmacyId.ToString(),
                p.Quantity,
                true
            ))
            .ToList();

        if (pharmaciesWithDrug.Count == 0)
            return NotFound("No drugs found.");

        return Ok(pharmaciesWithDrug);
    }
    
    /// <summary>
    /// Вывести информацию о средней стоимости препаратов каждой фармацевтической группе для каждой аптеки.
    /// </summary>
    [HttpGet("average-cost")]
    public async Task<ActionResult<IEnumerable<GetAverageCostPerGroupPerPharmacyDto>>> GetAverageCostPerGroupPerPharmacy()
    {
        var pharmacies = await pharmacyService.GetAsList();
        var positions = await positionsService.GetAsList();
        var prices = await priceService.GetAsList();
        var productGroups = await productGroupService.GetAsList();

        var averageCosts = 
            (from pharmacy in pharmacies
                join position in positions 
                    on pharmacy.Number equals position.PharmacyId
                where position.ProductGroupId.HasValue && position.PriceId.HasValue
                group position by new 
                { 
                    PharmacyName = pharmacy.Name, 
                    position.ProductGroupId 
                } 
                into grouped
                select new
                {
                    grouped.Key.PharmacyName,
                    PharmaceuticalGroupName = productGroups
                        .FirstOrDefault(pg => pg.Id == grouped.Key.ProductGroupId)?.Name,
                    AverageCost = grouped
                        .Where(p => p.PriceId.HasValue)
                        .Average(p => prices.FirstOrDefault(price => price.Id == p.PriceId)?.Cost ?? 0)
                }).ToList();

        if (averageCosts.Count == 0)
            return NotFound("No records found.");

        return Ok(averageCosts);
    }
    
    /// <summary>
    /// Вывести топ 5 аптек по количеству и объёму продаж данного препарата за указанный период времени.
    /// </summary>
    /// <param name="drugName">Drug name</param>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    [HttpGet("drug/{drugName}/top-pharmacies")]
    public async Task<ActionResult<IEnumerable<PharmacyAndNumericStatisticsDto>>> GetTopPharmaciesBySales(
        string drugName, DateTime startDate, DateTime endDate)
    {
        var positions = await positionsService.GetAll(p => p.Name == drugName);

        var prices = await priceService.GetAll(price => price.SellTime >= startDate && price.SellTime <= endDate);

        var topPharmacies = positions
            .Where(p => p is { Quantity: not null, PriceId: not null } && prices.Any(price => price.Id == p.PriceId))
            .Select(p =>
            {
                var price = prices.First(price => price.Id == p.PriceId);
                return new
                {
                    PharmacyName = p.PharmacyId.HasValue 
                        ? pharmacyService.GetByKey(p.PharmacyId.Value).Result?.Name 
                        : null,
                    SalesVolume = p.Quantity * price.Cost
                };
            })
            .Where(p => p.PharmacyName != null)
            .OrderByDescending(p => p.SalesVolume)
            .Take(5)
            .Select(p => new PharmacyAndNumericStatisticsDto(p.PharmacyName, p.SalesVolume, false))
            .ToList();

        // Если топ-5 аптек пуст, возвращаем NotFound
        if (topPharmacies.Count == 0)
            return NotFound("No sales found.");

        return Ok(topPharmacies);
    }
    
    /// <summary>
    /// Вывести список аптек указанного района, продавших заданный препарат более указанного объёма.
    /// </summary>
    /// <param name="drugName">Drug name</param>
    /// <param name="district">District</param>
    /// <param name="minVolume">Minimum volume</param>
    [HttpGet("drug/{drugName}/district/{district}/min-volume/{minVolume}")]
    public async Task<ActionResult<IEnumerable<string>>> GetPharmaciesByVolume(string drugName, string district, int minVolume)
    {
        var positions = await positionsService.GetAll(p => p.Name == drugName && p.Quantity > minVolume);
    
        var pharmacies = await pharmacyService.GetAsList();

        var pharmacyNames = positions
            .Where(p => p.PharmacyId.HasValue && pharmacies
                .Any(pharmacy => pharmacy.Number == p.PharmacyId && pharmacy.Address != null && pharmacy.Address.Contains(district)))
            .Select(p => pharmacies.First(pharmacy => pharmacy.Number == p.PharmacyId).Name)
            .Distinct()
            .ToList();

        if (!pharmacyNames.Any())
            return NotFound("No pharmacies found.");

        return Ok(pharmacyNames);
    }
    
    /// <summary>
    /// Вывести список аптек, в которых указанный препарат продается с минимальной ценой.
    /// </summary>
    /// <param name="drugName">Drug name</param>
    [HttpGet("drug/{drugName}/min-price")]
    public async Task<ActionResult<IEnumerable<string>>> GetPharmaciesWithMinPrice(string drugName)
    {
        var positions = await positionsService.GetAll(p => p.Name == drugName && p.PriceId.HasValue);
        var prices = await priceService.GetAsList();

        var minPrice = positions
            .Where(p => p.PriceId.HasValue)
            .Min(p => prices.FirstOrDefault(price => price.Id == p.PriceId)?.Cost ?? decimal.MaxValue);

        var pharmaciesWithMinPrice = positions
            .Where(p => p.PriceId.HasValue && prices.FirstOrDefault(price => price.Id == p.PriceId)?.Cost == minPrice)
            .Select(p => pharmacyService.GetByKey(p.PharmacyId!.Value).Result?.Name)
            .Where(pharmacyName => pharmacyName != null)
            .Distinct()
            .ToList();

        if (pharmaciesWithMinPrice.Count == 0)
            return NotFound("No pharmacies found.");

        return Ok(pharmaciesWithMinPrice);
    }
}