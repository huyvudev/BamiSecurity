using CR.Constants.Core.Order;
using CR.Core.ApplicationServices.OrderModule.Abstracts;
using CR.Core.Domain.Order;
using CR.Core.Dtos.OrderModule.SaleOrder;
using CR.DtoBase;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.OrderModule.Implements;

public class SaleOrderService : OrderService, ISaleOrderService
{
    private readonly IHandleOrderService _handleOrderService;

    public SaleOrderService(
        ILogger<SaleOrderService> logger,
        IHttpContextAccessor httpContext,
        IOrderStatusService orderStatusService,
        IHandleOrderService handleOrderService
    )
        : base(logger, httpContext, orderStatusService, handleOrderService)
    {
        _handleOrderService = handleOrderService;
    }

    private async Task<CoreOrder> CreateOrderBase(CreateSaleOrderDto input)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        var order = _dbContext
            .CoreOrders.Add(
                new CoreOrder
                {
                    Name = input.Name,
                    Address = input.Address,
                    OrderNumber = input.OrderNumber,
                    Namespace = input.Namespace,
                    City = input.City,
                    State = input.State,
                    PostalCode = input.PostalCode,
                    Country = input.Country,
                    Status = OrderStatus.PendingImported,
                }
            )
            .Entity;
        await _dbContext.SaveChangesAsync();

        foreach (var detail in input.Details)
        {
            _dbContext.CoreOrderDetails.Add(
                new CoreOrderDetail
                {
                    OrderId = order.Id,
                    MockUpFront = detail.MockUpFront,
                    MockUpBack = detail.MockUpBack,
                    SaleDesignFront = detail.SaleDesignFront,
                    SaleDesignBack = detail.SaleDesignBack,
                    SaleDesignSleeves = detail.SaleDesignSleeves,
                    SaleDesignHood = detail.SaleDesignHood,
                    Title = detail.Title,
                    Type = detail.Type,
                    Size = detail.Size,
                    Quantity = detail.Quantity,
                    SellerSku = detail.SellerSku,
                    Status = OrderDetailStatus.Pending,
                }
            );
        }
        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
        return order;
    }

    public async Task<Result> UploadOrder(CreateSaleOrderDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(UploadOrder),
            nameof(input),
            input
        );

        var order = await CreateOrderBase(input);

        // Enqueue the method from IHandleOrderService
        var backgroundJob = BackgroundJob.Enqueue(() => _handleOrderService.HandleOrder(order.Id));
        order.BackgroundJobId = backgroundJob;
        await _dbContext.SaveChangesAsync();
        return Result.Success(new CreateSaleOrderResultDto { Id = order.Id });
    }

    private void CreateOrderItem(CoreOrder order)
    {
        int itemIndex = 0;
        foreach (var detail in order.OrderDetails)
        {
            for (int i = 0; i < detail.Quantity; i++)
            {
                itemIndex++;
                _dbContext.CoreOrderItems.Add(
                    new CoreOrderItem
                    {
                        ItemIndex = itemIndex,
                        OrderId = order.Id,
                        Status = OrderItemStatus.Unprinted,
                    }
                );
            }
        }
        _dbContext.SaveChanges();
    }

    public async Task<Result<CreateSaleOrderResultDto>> CreateOrder(CreateSaleOrderDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(UploadOrder),
            nameof(input),
            input
        );

        var order = await CreateOrderBase(input);
        //tạo order item
        order.Status = OrderStatus.PendingDesign;
        CreateOrderItem(order);
        await _dbContext.SaveChangesAsync();
        return Result.Success(new CreateSaleOrderResultDto { Id = order.Id });
    }

    public async Task<Result> FindAllOrder(PagingRequestBaseDto input)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> FindOrderById(int orderId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> PushOrder(int orderId)
    {
        throw new NotImplementedException();
    }
}
