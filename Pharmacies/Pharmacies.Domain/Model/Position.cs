using Pharmacies.Model.Reference;

namespace Pharmacies.Model;

public class Position
{
    public required int Code { get; init; }
    public string? Name { get; set; }
    public ProductGroup? ProductGroup { get; set; }
    public List<PharmaceuticalGroup> PharmaceuticalGroups { get; set; } = [];
    public int? Quantity { get; set; }
    public Pharmacy? Pharmacy { get; set; }
    public Price? Price { get; set; }
}