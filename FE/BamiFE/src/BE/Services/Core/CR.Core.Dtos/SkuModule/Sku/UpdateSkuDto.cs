using System.ComponentModel.DataAnnotations;

namespace CR.Core.Dtos.SkuModule.Sku;
public class UpdateSkuDto
{
    public int Id { get; set; }
    #region trường cơ bản
    /// <summary>
    /// Mã code SKU ví dụ có thể tự điền 4L_WO_FRAME_12MT hoăc ghép từ SkuBase.Code_Material.Code_Material.Code
    /// </summary>
    [MaxLength(512)]
    public required string Code { get; set; }

    /// <summary>
    /// Mô tả SKU ví dụ với SKU 4L_WO_FRAME_12MT thì mô tả chi tiết nó 2 lít, có không có khung,  12 mã nội bộ, TM chất liệu
    /// </summary>
    [MaxLength(512)]
    public string? Description { get; set; }
    #endregion

    #region các trường bool
    /// <summary>
    /// có phải loại khổ lớn hay không
    /// </summary>
    public bool IsBigSize { get; set; }

    /// <summary>
    /// Có active hay không
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Cần review hay không
    /// </summary>
    public bool NeedToReview { get; set; }

    /// <summary>
    ///có cần quản lý nguyên liệu không
    /// </summary>
    public bool NeedManageMaterials { get; set; }

    /// <summary>
    ///cho phép đối tác đánh dấu QC không
    /// </summary>
    public bool AllowPartnerMarkQc { get; set; }

    /// <summary>
    ///cho phép QC nhiều item
    /// </summary>
    public bool AllowQcMultipleItems { get; set; }
    #endregion

    #region Link với SkuBase
    /// <summary>
    /// Id SkuBase
    /// </summary>
    public int? SkuBaseId { get; set; }
    #endregion

    #region Link với Materia
    /// <summary>
    /// Id MaterialId
    /// </summary>
    public int? MaterialId { get; set; }
    #endregion

    #region Link với Product Method
    /// <summary>
    /// Id ProductMethodId
    /// </summary>
    public int? ProductMethodId { get; set; }
    #endregion
}
