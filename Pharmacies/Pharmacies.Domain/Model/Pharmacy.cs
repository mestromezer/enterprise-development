namespace Pharmacies.Model;

public class Pharmacy
{
    public required int Number { get; init; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? DirectorFullName { get; set; }
}