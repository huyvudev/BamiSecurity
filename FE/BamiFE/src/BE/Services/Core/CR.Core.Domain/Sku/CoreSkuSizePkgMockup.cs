using CR.Constants.Common.Database;

namespace CR.Core.Domain.Sku;

/// <summary>
/// Đây là các hình ảnh hướng dẫn đóng gói
/// </summary>
[Table(nameof(CoreSkuSizePkgMockup), Schema = DbSchemas.CRCore)]
public class CoreSkuSizePkgMockup
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Đường dẫn của Mockup
    /// </summary>
    [MaxLength(512)]
    public required string MockupUrl { get; set; }

    #region Link với SkuSize
    /// <summary>
    /// Sku Size
    /// </summary>
    public int SkuSizeId { get; set; }
    public CoreSkuSize SkuSize { get; set; } = null!;
    #endregion
}
