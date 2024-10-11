using Microsoft.AspNetCore.Mvc;
using Pharmacies.Interfaces;
using Pharmacies.Model;

namespace Pharmacies.Controllers;

/// <summary>
/// Контроллер с функционалом аналитических запросов
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AnalyticsController(
    IRepository<Position, int> positionsRepository
    ) : ControllerBase
{
    /// <summary>
    ///  Вывести сведения о всех препаратах в заданной аптеке, упорядочить по названию.
    /// </summary>
    /// <param name="pharmacyNumber">Идентификатор аптеки</param>
    [HttpGet("pharmacy/{pharmacyNumber}/positions")]
    public async Task<ActionResult<IEnumerable<Position>>> GetPositionsByPharmacy(int pharmacyNumber)
    {
        var positions = (await positionsRepository.GetAsList())
            .Where(p => p.Pharmacy?.Number == pharmacyNumber)
            .OrderBy(p => p.Name)
            .ToList();

        if (positions.Count == 0)
            return NotFound("Препараты не найдены.");

        return Ok(positions);
    }

    /// <summary>
    /// Вывести для данного препарата подробный список всех аптек с указанием количества препарата в аптеках.
    /// </summary>
    /// <param name="drugName">Название препарата</param>
    [HttpGet("drug/{drugName}/pharmacies")]
    public async Task<ActionResult<IEnumerable<object>>> GetPharmaciesWithDrugQuantity(string drugName)
    {
        var pharmaciesWithDrug = (await positionsRepository.GetAsList())
            .Where(p => p.Name == drugName)
            .Select(p => new { p.Pharmacy?.Name, p.Quantity })
            .ToList();

        if (pharmaciesWithDrug.Count == 0)
            return NotFound("Препараты не найдены.");

        return Ok(pharmaciesWithDrug);
    }

    /// <summary>
    /// Вывести информацию о средней стоимости препаратов каждой фармацевтической группе для каждой аптеки.
    /// </summary>
    [HttpGet("average-cost")]
    public async Task<ActionResult<IEnumerable<object>>> GetAverageCostPerGroupPerPharmacy()
    {
        var averageCosts =
            (from position in (await positionsRepository.GetAsList())
                from pharmaceuticalGroup in position.PharmaceuticalGroups
                group position by new { Pharmacy = position.Pharmacy?.Name, Group = pharmaceuticalGroup.Name }
                into grouped
                select new
                {
                    grouped.Key.Pharmacy,
                    grouped.Key.Group,
                    AverageCost = grouped.Average(p => p.Price?.Cost)
                }).ToList();

        if (averageCosts.Count == 0)
            return NotFound("Не найдено записей для расчета.");

        return Ok(averageCosts);
    }

    /// <summary>
    /// Вывести топ 5 аптек по количеству и объёму продаж данного препарата за указанный период времени.
    /// </summary>
    /// <param name="drugName">Название препарата</param>
    /// <param name="startDate">Дата начиная с</param>
    /// <param name="endDate">Дата заканчивая</param>
    [HttpGet("drug/{drugName}/top-pharmacies")]
    public async Task<ActionResult<IEnumerable<object>>> GetTopPharmaciesBySales(string drugName, DateTime startDate,
        DateTime endDate)
    {
        var topPharmacies = (await positionsRepository.GetAsList())
            .Where(p => p.Name == drugName && p.Price?.SellTime >= startDate && p.Price.SellTime <= endDate)
            .OrderByDescending(p => p.Quantity * p.Price?.Cost)
            .Take(5)
            .Select(p => new { p.Pharmacy?.Name, TotalVolume = p.Quantity * p.Price?.Cost })
            .ToList();

        if (topPharmacies.Count == 0)
            return NotFound("Продажи не найдены.");

        return Ok(topPharmacies);
    }

    /// <summary>
    /// Вывести список аптек указанного района, продавших заданный препарат более указанного объёма.
    /// </summary>
    /// <param name="drugName">Название препарата</param>
    /// <param name="district">Название района</param>
    /// <param name="minVolume">Минимальный объем</param>
    [HttpGet("drug/{drugName}/district/{district}/min-volume/{minVolume}")]
    public async Task<ActionResult<IEnumerable<object>>> GetPharmaciesByVolume(string drugName, string district, int minVolume)
    {
        var pharmacies = (await positionsRepository.GetAsList())
            .Where(p =>
                p is { Pharmacy.Address: not null, Pharmacy: not null }
                && p.Pharmacy.Address.Contains(district)
                && p.Quantity > minVolume
                && p.Name == drugName)
            .Select(p => p.Pharmacy!.Name)
            .Distinct()
            .ToList();

        if (!pharmacies.Any())
            return NotFound("Аптеки не найдены.");

        return Ok(pharmacies);
    }

    /// <summary>
    /// Вывести список аптек, в которых указанный препарат продается с минимальной ценой.
    /// </summary>
    /// <param name="drugName">Название препарата</param>
    [HttpGet("drug/{drugName}/min-price")]
    public async Task<ActionResult<IEnumerable<object>>> GetPharmaciesWithMinPrice(string drugName)
    {
        var positions = (await positionsRepository.GetAsList());
        
        var minPrice = positions
            .Where(p => p.Name == drugName)
            .Min(p => p.Price?.Cost);

        var pharmaciesWithMinPrice = positions
            .Where(p => p is { Price: not null } && p.Price.Cost == minPrice && p.Name == drugName)
            .Select(p => p.Pharmacy?.Name)
            .Distinct()
            .ToList();

        if (!pharmaciesWithMinPrice.Any())
            return NotFound("Аптеки с минимальной ценой не найдены.");

        return Ok(pharmaciesWithMinPrice);
    }
}