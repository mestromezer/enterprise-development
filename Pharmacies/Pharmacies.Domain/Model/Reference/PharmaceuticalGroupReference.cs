namespace Pharmacies.Model.Reference;

public class PharmaceuticalGroupReference
{
    public required int Id { get; set; }

    public int? PharmaceuticalGroupId { get; set; }

    public virtual PharmaceuticalGroup? PharmaceuticalGroup { get; set; }
    
    public int? PositionId { get; set; }
    
    public virtual Position? Position { get; set; }
}