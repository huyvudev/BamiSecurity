using System.ComponentModel.DataAnnotations;

namespace CR.Core.Dtos.PartnerModule.Partner;

public class CreatePartnerDto
{
    [MaxLength(512)]
    public required string Name { get; set; }
    public int PartnerTypeId { get; set; }
}
