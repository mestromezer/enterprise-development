namespace Pharmacies.Application.Dto;

public class PharmacyDto
{
    public required int Number { get; init; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? DirectorFullName { get; set; }
}