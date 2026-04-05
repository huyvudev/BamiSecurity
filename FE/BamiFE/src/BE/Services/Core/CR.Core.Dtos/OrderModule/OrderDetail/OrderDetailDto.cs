namespace CR.Core.Dtos.OrderModule.OrderDetail;

public class OrderDetailDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string? Type { get; set; }

    public string? Title { get; set; }

    public string? Size { get; set; }

    public string? SellerSku { get; set; }

    public string? Color { get; set; }

    public int Quantity { get; set; } = 1;

    #region Mockup

    public string? MockUpFront { get; set; }
    public string? ErrorMessage { get; set; }

    public string? MockUpBack { get; set; }
    #endregion

    #region DesignSale

    public string? SaleDesignFront { get; set; }

    public string? SaleDesignBack { get; set; }

    public string? SaleDesignSleeves { get; set; }

    public string? SaleDesignHood { get; set; }
    #endregion
}
