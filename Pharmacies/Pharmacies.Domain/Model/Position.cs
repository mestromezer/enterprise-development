using Pharmacies.Model.Reference;

namespace Pharmacies.Model;

public class Position
{
    public int? Code { get; set; }
    public string? Name { get; set; }
    public ProductGroup? ProductGroup { get; set; }
    public List<PharmaceuticalGroup> PharmaceuticalGroups { get; set; } = [];
    public int? Quantity { get; set; }
}