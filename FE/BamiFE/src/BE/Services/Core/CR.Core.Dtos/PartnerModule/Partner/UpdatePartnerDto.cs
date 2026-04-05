namespace CR.Core.Dtos.PartnerModule.Partner;

public class UpdatePartnerDto
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int PartnerTypeId { get; set; }
}
