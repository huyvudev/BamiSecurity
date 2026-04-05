using System.ComponentModel.DataAnnotations;

namespace CR.Core.Dtos.SkuModule.SkuBase;
public class CreateSkuBaseDto
{
    /// <summary>
    /// Ví dụ là 1L_WOODSTAND để tý nữa map với Material , ProductionMethod để cho ra mã SKU hoàn chỉnh 1L_WOODSTAND_0205TM
    /// </summary>
    [MaxLength(512)]
    public required string Code { get; set; }

    /// <summary>
    /// Mô tả
    /// </summary>
    [MaxLength(512)]
    public string? Description { get; set; }
}
