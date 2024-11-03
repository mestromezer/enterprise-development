using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacies.Model.Reference;

public class PharmaceuticalGroupReference
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }

    public int? PharmaceuticalGroupId { get; set; }

    public virtual PharmaceuticalGroup? PharmaceuticalGroup { get; set; }
    
    public int? PositionId { get; set; }
    
    public virtual Position? Position { get; set; }
}