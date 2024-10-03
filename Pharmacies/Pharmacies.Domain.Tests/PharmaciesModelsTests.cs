using Xunit;
using Seed = Pharmacies.Domain.Tests.PharmaciesModelsTestsDataSeed;

namespace Pharmacies.Domain.Tests;

public class Tests
{
   [Fact]
    public void ShouldReturnAllPositionsForSpecificPharmacyOrderedByName()
    {
        // Arrange
        const int pharmacyNumber = 1;
        const int expectedCount = 2;

        // Act
        var positions = Seed.Positions
            .Where(p => p.Pharmacy?.Number == pharmacyNumber)
            .OrderBy(p => p.Name)
            .ToList();

        // Assert
        Assert.NotEmpty(positions);
        Assert.True(positions.SequenceEqual(positions.OrderBy(p => p.Name)));
        Assert.Equal(expectedCount, positions.Count);
    }

    [Fact]
    public void ShouldReturnAllPharmaciesWithSpecificDrugAndTheirQuantities()
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
        Assert.Single(pharmaciesWithDrug);
    }

    [Fact]
    public void ShouldReturnAverageCostOfDrugsPerPharmaceuticalGroupForEachPharmacy()
    {
        const decimal expectedAvg = 2042.5m;

        // Act
        var averageCosts = 
            (from position in Seed.Positions
                from pharmaceuticalGroup in position.PharmaceuticalGroups
                group position by new { Pharmacy = position.Pharmacy?.Name, Group = pharmaceuticalGroup.Name } into grouped
                select new
                {
                    grouped.Key.Pharmacy,
                    grouped.Key.Group,
                    AverageCost = grouped.Average(p => p.Price?.Cost)
                }).ToList();

        // Assert
        Assert.NotEmpty(averageCosts);
        averageCosts.ForEach(avgCost => Assert.NotNull(avgCost.Group));
        Assert.Equal(expectedAvg, averageCosts.Sum(rec => rec.AverageCost));
    }


    [Fact]
    public void ShouldReturnTop5PharmaciesBySalesVolumeWithinPeriod()
    {
        // Arrange
        const decimal expectedVolume = 7500m;
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
        Assert.Equal(topPharmacies.Sum(p => p.TotalVolume), expectedVolume);
    }

    [Fact]
    public void ShouldReturnPharmacyListInSpecificAreaThatSoldDrugMoreThanGivenVolume()
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
    public void ShouldReturnPharmacyListThatSellsDrugAtMinimumPrice()
    {
        // Arrange
        const string expectedMinName = "Аптека №1";
        const string drugName = "Аспирин";

        // Act
        var minPrice = Seed.Positions
            .Where(p => p.Name == drugName)
            .Min(p => p.Price?.Cost);

        var pharmaciesWithMinPrice = Seed.Positions
            .Where(p => p is { Price: not null, Name: drugName } && p.Price.Cost == minPrice)
            .Select(p => p.Pharmacy?.Name)
            .Distinct()
            .ToList();

        // Assert
        Assert.NotEmpty(pharmaciesWithMinPrice);
        Assert.True(pharmaciesWithMinPrice.Count > 0);
        Assert.True(expectedMinName == pharmaciesWithMinPrice.First());
    }
}