namespace CR.Core.Dtos.SkuModule.SkuBase;
public class SkuBaseDto
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public string? Description { get; set; }
}
