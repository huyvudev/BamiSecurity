using CR.Constants.Common.Database;
using CR.EntitiesBase.Interfaces;

namespace CR.Core.Domain.Sku;

[Table(nameof(CoreProductionMethod), Schema = DbSchemas.CRCore)]
public class CoreProductionMethod
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Tên ví dụ Trắng Màu
    /// </summary>
    [MaxLength(512)]
    public required string Name { get; set; }

    /// <summary>
    /// code ví dụ TM  để tý nữa map với Material , SkuBase để cho ra mã SKU hoàn chỉnh 1L_WOODSTAND_0205TM
    /// </summary>
    [MaxLength(512)]
    public required string Code { get; set; }

    #region Link với Sku
    public List<CoreSku> Skus { get; set; } = [];
    #endregion
}
