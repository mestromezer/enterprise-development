namespace Pharmacies.Application.Dto;

public class PositionDto
{
    public required int Code { get; init; }
    
    public string? Name { get; set; }
    
    public int? ProductGroupId { get; set; }
    
    public int? Quantity { get; set; }
    
    public int? PharmacyId { get; set; }
    
    public int? PriceId { get; set; }
}