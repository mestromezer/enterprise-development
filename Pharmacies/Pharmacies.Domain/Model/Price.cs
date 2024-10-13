namespace Pharmacies.Model;

public class Price
{
    public required int Id { get; set; }
    public string? Manufacturer { get; set; }
    public DateTime? ProductionTime { get; set; }
    public bool? IfCash { get; set; }
    public string? SellerOrganizationName { get; set; }
    public required decimal Cost { get; set; }
    public required DateTime SellTime { get; set; }
}