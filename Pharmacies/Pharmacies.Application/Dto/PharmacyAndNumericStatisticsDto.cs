namespace Pharmacies.Application.Dto;

public record PharmacyAndNumericStatisticsDto(string? Name, decimal? Value, bool IfInteger);