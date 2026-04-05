namespace CR.Core.Dtos.SkuModule.Material;

public class MaterialDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public string? Description { get; set; }
}
