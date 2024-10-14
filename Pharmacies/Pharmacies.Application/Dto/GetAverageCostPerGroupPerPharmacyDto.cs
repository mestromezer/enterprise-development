namespace Pharmacies.Application.Dto;

public record GetAverageCostPerGroupPerPharmacyDto(string Pharmacy, string? Group, decimal? AverageCost);