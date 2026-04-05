using CR.Constants.Common.Database;

namespace CR.Core.Domain.Brand;

/// <summary>
/// Cửa hàng
/// </summary>
[Table(nameof(CoreStore), Schema = DbSchemas.CRCore)]
public class CoreStore
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Mã này chính là namespace ở OrderItem
    /// </summary>
    [MaxLength(512)]
    public required string Name { get; set; }

    #region Link với Brand
    /// <summary>
    /// Link với Brand
    /// </summary>
    public int BrandId { get; set; }

    public CoreBrand Brand { get; set; } = null!;
    #endregion
}
