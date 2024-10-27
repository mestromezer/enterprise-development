namespace Pharmacies.Application.Dto.AnalyticsResults;

public record GetAverageCostPerGroupPerPharmacyDto(string Pharmacy, string? Group, decimal? AverageCost);