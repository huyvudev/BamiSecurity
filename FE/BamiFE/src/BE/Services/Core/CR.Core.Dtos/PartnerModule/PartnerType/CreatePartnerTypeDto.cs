using System.ComponentModel.DataAnnotations;

namespace CR.Core.Dtos.PartnerModule.PartnerType;

public class CreatePartnerTypeDto
{
    [MaxLength(512)]
    public required string Name { get; set; }
}
