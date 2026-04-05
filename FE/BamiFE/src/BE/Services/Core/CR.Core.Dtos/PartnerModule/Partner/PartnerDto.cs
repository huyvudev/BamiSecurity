namespace CR.Core.Dtos.PartnerModule.Partner;

public class PartnerDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int PartnerTypeId { get; set; }
    public required string NamePartnerType { get; set; }
    public DateTime? CreatedDate { get; set; }
}
