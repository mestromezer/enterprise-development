using Xunit;
using Seed = Pharmacies.Domain.Tests.PharmaciesModelsTestsDataSeed;

namespace Pharmacies.Domain.Tests;

public class Tests
{
    [Fact]
    public void Should_Return_AllPositionsForSpecificPharmacy_OrderedByName()
    {
        // Arrange
        var pharmacy = Seed.Pharmacies.First(p => p.Number == 1);

        // Act
        var positions = Seed.Positions
            .Where(p => p.Pharmacy?.Number == pharmacy.Number)
            .OrderBy(p => p.Name)
            .ToList();

        // Assert
        Assert.NotEmpty(positions);
        Assert.True(positions.SequenceEqual(positions.OrderBy(p => p.Name)));
    }

    [Fact]
    public void Should_Return_AllPharmaciesWithSpecificDrug_AndTheirQuantities()
    {
        // Arrange
        const string drugName = "Аспирин";

        // Act
        var pharmaciesWithDrug = Seed.Positions
            .Where(p => p.Name == drugName)
            .Select(p => new { p.Pharmacy?.Name, p.Quantity })
            .ToList();

        // Assert
        Assert.NotEmpty(pharmaciesWithDrug);
        pharmaciesWithDrug.ForEach(ph => Assert.NotNull(ph.Name));
    }

    [Fact]
    public void Should_Return_AverageCostOfDrugsPerPharmaceuticalGroup_ForEachPharmacy()
    {
        // Act
        var averageCosts = Seed.Positions
            .GroupBy(p => 
                new { p.Pharmacy?.Name, PharmaceuticalGroup = p.PharmaceuticalGroups.FirstOrDefault()?.Name }
            )
            .Select(g => new
            {
                Pharmacy = g.Key.Name,
                PharmaceuticalGroup = g.Key.PharmaceuticalGroup,
                AverageCost = g.Average(p => p.Price?.Cost)
            })
            .ToList();

        // Assert
        Assert.NotEmpty(averageCosts);
        Assert.NotEmpty(averageCosts);
    }

    [Fact]
    public void Should_Return_Top5PharmaciesBySalesVolume_WithinPeriod()
    {
        // Arrange
        const string drugName = "Аспирин";
        var startDate = new DateTime(2023, 9, 1);
        var endDate = new DateTime(2023, 9, 30);

        // Act
        var topPharmacies = Seed.Positions
            .Where(p => p.Name == drugName && p.Price?.SellTime >= startDate && p.Price.SellTime <= endDate)
            .OrderByDescending(p => p.Quantity * p.Price?.Cost)
            .Take(5)
            .Select(p => new { p.Pharmacy?.Name, TotalVolume = p.Quantity * p.Price?.Cost })
            .ToList();

        // Assert
        Assert.True(topPharmacies.Count <= 5);
    }

    [Fact]
    public void Should_Return_PharmacyList_InSpecificArea_ThatSoldDrugMoreThanGivenVolume()
    {
        // Arrange
        const string drugName = "Аспирин";
        const int minimumVolume = 20;
        const string area = "Красноглинский";
        const string targetName = "Аптека №1";

        // Act
        var pharmacies = Seed.Positions
            .Where(p =>
                p is { Name: drugName, Pharmacy.Address: not null, Pharmacy: not null }
                && p.Pharmacy.Address.Contains(area)
                && p.Quantity is > minimumVolume)
            .Select(p => p.Pharmacy!.Name)
            .Distinct()
            .ToList();

        // Assert
        Assert.NotEmpty(pharmacies);
        Assert.All(pharmacies, pharmacyName => Assert.Contains(targetName, pharmacyName));
    }

    [Fact]
    public void Should_Return_PharmacyList_ThatSellsDrugAtMinimumPrice()
    {
        // Arrange
        const string drugName = "Аспирин";

        // Act
        var minPrice = Seed.Positions
            .Where(p => p.Name == drugName)
            .Min(p => p.Price?.Cost);

        var pharmaciesWithMinPrice = Seed.Positions
            .Where(p => p.Price != null && p.Name == drugName && p.Price.Cost == minPrice)
            .Select(p => p.Pharmacy?.Name)
            .Distinct()
            .ToList();

        // Assert
        Assert.NotEmpty(pharmaciesWithMinPrice);
        Assert.True(pharmaciesWithMinPrice.Count > 0);
    }
}