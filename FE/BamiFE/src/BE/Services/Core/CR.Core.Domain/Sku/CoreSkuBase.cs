using CR.Constants.Common.Database;

namespace CR.Core.Domain.Sku;

[Table(nameof(CoreSkuBase), Schema = DbSchemas.CRCore)]
public class CoreSkuBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Ví dụ là 1L_WOODSTAND để tý nữa map với Material , ProductionMethod để cho ra mã SKU hoàn chỉnh 1L_WOODSTAND_0205TM
    /// </summary>
    [MaxLength(512)]
    public required string Code { get; set; }

    [MaxLength(512)]
    public string? Description { get; set; }

    #region Link với SKu
    public List<CoreSku> Skus { get; set; } = [];
    #endregion
}
