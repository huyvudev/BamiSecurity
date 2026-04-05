namespace CR.Core.Dtos.SkuModule.Sku;
public class SkuDto
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public string? Description { get; set; }
    public bool IsBigSize { get; set; }
    public bool IsActive { get; set; }
    public bool NeedToReview { get; set; }
    public bool NeedManageMaterials { get; set; }
    public bool AllowPartnerMarkQc { get; set; }
    public bool AllowQcMultipleItems { get; set; }
    public int? SkuBaseId { get; set; }
    public int? MaterialId { get; set; }
    public int? ProductMethodId { get; set; }
}
