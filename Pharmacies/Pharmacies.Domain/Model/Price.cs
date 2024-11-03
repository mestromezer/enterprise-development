using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacies.Model;

public class Price
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }
    
    public string? Manufacturer { get; set; }
    
    public bool? IfCash { get; set; }
    
    public string? SellerOrganizationName { get; set; }
    
    public DateTime? ProductionTime { get; set; }
    
    public required decimal Cost { get; set; }
    
    public required DateTime SellTime { get; set; }
}