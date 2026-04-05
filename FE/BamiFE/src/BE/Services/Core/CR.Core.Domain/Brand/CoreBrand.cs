using CR.Constants.Common.Database;

namespace CR.Core.Domain.Brand;

/// <summary>
/// Nhãn hiệu quản lý trong 1 màn hình
/// </summary>
[Table(nameof(CoreBrand), Schema = DbSchemas.CRCore)]
public class CoreBrand
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Tên của nhãn hiệu
    /// </summary>
    [MaxLength(512)]
    public required string Name { get; set; }

    #region Link với Storse
    public List<CoreStore> Stores { get; set; } = [];
    #endregion
}
