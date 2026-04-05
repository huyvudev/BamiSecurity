using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CR.DtoBase.Validations;

namespace CR.Core.Dtos.OrderModule.OrderDetail;

public class OrderDetailDoneDto : OrderDetailBaseDto
{
    /// <summary>
    /// Chiều rộng
    /// </summary>
    public double? Width { get; set; }

    /// <summary>
    /// Chiều dài
    /// </summary>
    public double? Length { get; set; }

    /// <summary>
    /// Mã Sku
    /// </summary>
    public int? SkuId { get; set; }

    #region Design
    /// <summary>
    /// Thiết kế mặt trước
    /// </summary>
    [CustomMaxLength(512)]
    public string? DesignFront { get; set; }

    /// <summary>
    /// Thiết kế mặt sau
    /// </summary>
    [CustomMaxLength(512)]
    public string? DesignBack { get; set; }

    /// <summary>
    /// Thiết kế tay áo
    /// </summary>
    [CustomMaxLength(512)]
    public string? DesignSleeves { get; set; }

    /// <summary>
    /// Thiết kế mũ
    /// </summary>
    [CustomMaxLength(512)]
    public string? DesignHood { get; set; }
    #endregion
}
