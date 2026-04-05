using System.ComponentModel.DataAnnotations;

namespace CR.Core.Dtos.SkuModule.SkuSizePkgMockup;
public class CreateSkuSizePkgMockupDto
{

    /// <summary>
    /// Đường dẫn của Mockup
    /// </summary>
    [MaxLength(512)]
    public required string MockupUrl { get; set; }

    /// <summary>
    /// Sku Size
    /// </summary>
    public int? SkuSizeId { get; set; }
}
