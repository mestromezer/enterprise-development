namespace Pharmacies.Application.Dto.AnalyticsResults;

public record PharmacyAndNumericStatisticsDto(string? Name, decimal? Value, bool IfInteger);