using CR.Constants.Common.Database;
using CR.EntitiesBase.Interfaces;

namespace CR.Core.Domain.Sku;

[Table(nameof(CoreMaterial), Schema = DbSchemas.CRCore)]
public class CoreMaterial
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Tên ví dụ Gỗ 5 ly
    /// </summary>
    [MaxLength(512)]
    public required string Name { get; set; }

    /// <summary>
    /// Code ví dụ 0205 để tý nữa map với ProductMethod , SkuBase để cho ra mã SKU hoàn chỉnh 1L_WOODSTAND_0205TM
    /// </summary>
    [MaxLength(512)]
    public required string Code { get; set; }

    /// <summary>
    /// Mô tả ví dụ Gỗ 5 ly chất lượng cao ,....
    /// </summary>
    [MaxLength(512)]
    public string? Description { get; set; }

    #region Link với Sku
    public List<CoreSku> Skus { get; set; } = [];
    #endregion
}
