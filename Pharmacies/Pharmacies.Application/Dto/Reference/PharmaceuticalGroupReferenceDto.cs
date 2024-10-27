namespace Pharmacies.Application.Dto.Reference;

public class PharmaceuticalGroupReferenceDto
{
    public required int Id { get; set; }
    
    public required int PharmaceuticalGroupId { get; set; }
    
    public required int PositionId { get; set; }
}