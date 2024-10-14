using Pharmacies.Model.Reference;

namespace Pharmacies.Model;

/// <summary>
/// Полe PharmaceuticalGroups планирую собирать через fluent api конфиг EF
/// По этому сервис PharmaceuticalGroupReferenceService пока что бесполезен
/// </summary>
public class Position
{
    public required int Code { get; set; }
    public string? Name { get; set; }
    public virtual ProductGroup? ProductGroup { get; set; }
    public virtual List<PharmaceuticalGroup?> PharmaceuticalGroups { get; set; } = [];
    public int? Quantity { get; set; }
    public virtual Pharmacy? Pharmacy { get; set; }
    public virtual Price? Price { get; set; }
}