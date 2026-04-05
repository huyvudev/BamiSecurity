namespace CR.Core.Domain.Order;

public abstract class CoreOrderDetailBase
{
    #region Link với Order
    /// <summary>
    /// Trong Order nào
    /// </summary>
    public int OrderId { get; set; }

    public CoreOrder Order { get; set; } = null!;
    #endregion

    #region Mockup
    /// <summary>
    /// Mockup mặt trước
    /// </summary>
    [MaxLength(512)]
    public string? MockUpFront { get; set; }

    /// <summary>
    /// mockup mặt sau
    /// </summary>
    [MaxLength(512)]
    public string? MockUpBack { get; set; }
    #endregion


    #region Design của sale
    /// <summary>
    /// Thiết kế mặt trước
    /// </summary>
    [MaxLength(512)]
    public string? SaleDesignFront { get; set; }

    /// <summary>
    /// Thiết kế mặt sau
    /// </summary>
    [MaxLength(512)]
    public string? SaleDesignBack { get; set; }

    /// <summary>
    /// Thiết kế tay áo
    /// </summary>
    [MaxLength(512)]
    public string? SaleDesignSleeves { get; set; }

    /// <summary>
    /// Thiết kế mũ
    /// </summary>
    [MaxLength(512)]
    public string? SaleDesignHood { get; set; }
    #endregion

    #region Design của admin
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
    #endregion
}
