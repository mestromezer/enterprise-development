using Pharmacies.Model.Reference;

namespace Pharmacies.Model;

public class Position
{
    public required int Code { get; set; }
    
    public string? Name { get; set; }

    public int? ProductGroupId { get; set; }

    public virtual ProductGroup? ProductGroup { get; set; }
    
    public int? Quantity { get; set; }

    public int? PharmacyId { get; set; }

    public virtual Pharmacy? Pharmacy { get; set; }

    public int? PriceId { get; set; }

    public virtual Price? Price { get; set; }
}