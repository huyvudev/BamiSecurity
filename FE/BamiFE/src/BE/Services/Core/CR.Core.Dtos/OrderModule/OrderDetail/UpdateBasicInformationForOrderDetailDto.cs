using System.ComponentModel.DataAnnotations;

namespace CR.Core.Dtos.OrderModule.OrderDetail;

public class UpdateBasicInformationForOrderDetailDto : UpdateOrderDetailBaseDto
{
    public double Width { get; set; }
    public double Length { get; set; }
    public int? SkuId { get; set; }

    /// <summary>
    /// Thiết kế mặt trước
    /// </summary>
    [MaxLength(512)]
    public string? DesignFront { get; set; }

    /// <summary>
    /// Thiết kế mặt sau
    /// </summary>
    [MaxLength(512)]
    public string? DesignBack { get; set; }

    /// <summary>
    /// Thiết kế tay áo
    /// </summary>
    [MaxLength(512)]
    public string? DesignSleeves { get; set; }

    /// <summary>
    /// Thiết kế mũ
    /// </summary>
    [MaxLength(512)]
    public string? DesignHood { get; set; }

}
