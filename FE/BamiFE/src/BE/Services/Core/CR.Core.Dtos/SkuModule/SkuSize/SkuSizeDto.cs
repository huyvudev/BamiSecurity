namespace CR.Core.Dtos.SkuModule.SkuSize;
public class SkuSizeDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double Length { get; set; }
    public double Weight { get; set; }
    public double AdditionalWeight { get; set; }
    public bool IsDefault { get; set; }
    public double BaseCost { get; set; }
    public double? CostInMeters { get; set; }
    public double? WeightByVolume { get; set; }
    public string? PackageDescription { get; set; }
    public int SkuId { get; set; }
}
