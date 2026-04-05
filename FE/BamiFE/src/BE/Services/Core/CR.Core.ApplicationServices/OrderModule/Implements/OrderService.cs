using CR.ApplicationBase.Common;
using CR.Constants.Core.Order;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.Common;
using CR.Core.ApplicationServices.OrderModule.Abstracts;
using CR.Core.Domain.Order;
using CR.Core.Dtos.OrderModule.Order;
using CR.Core.Dtos.OrderModule.OrderDetail;
using CR.Core.Dtos.OrderModule.OrderDto;
using CR.DtoBase;
using CR.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.OrderModule.Implements;

public class OrderService : CoreServiceBase, IOrderService
{
    public OrderService(
        ILogger<OrderService> logger,
        IHttpContextAccessor httpContext,
        IOrderStatusService orderStatusService,
        IHandleOrderService handleOrderService
    )
        : base(logger, httpContext) { }

    public async Task<Result<CreateOrderResultDto>> CreateOrder(CreateOrderDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(CreateOrder),
            nameof(input),
            input
        );
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        if (await _dbContext.CoreOrders.AnyAsync(e => e.OrderNumber == input.OrderNumber))
        {
            return Result<CreateOrderResultDto>.Failure(
                CoreErrorCode.CoreOrderNumberHasBeenUsed,
                this.GetCurrentMethodInfo()
            );
        }

        var order = _dbContext
            .CoreOrders.Add(
                new CoreOrder
                {
                    Name = input.Name,
                    Address = input.Address,
                    OrderNumber = input.OrderNumber,
                    Address2 = input.Address2,
                    BrandId = input.BrandId,
                    Email = input.Email,
                    Phone = input.Phone,
                    Tax = input.Tax,
                    City = input.City,
                    State = input.State,
                    PostalCode = input.PostalCode,
                    Country = input.Country,
                    Namespace = input.Namespace,

                    Status = OrderStatus.PendingImported, // Trạng thái đơn hàng khi mới tạo
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
                    Color = detail.Color,
                    Type = detail.Type,
                    Size = detail.Size,
                    Quantity = detail.Quantity,
                    SellerSku = detail.SellerSku,
                    Status = OrderDetailStatus.Done, // Trạng thái chi tiết đơn hàng khi mới tạo
                }
            );
        }

        // Lưu tất cả thay đổi (orders và order details)
        await _dbContext.SaveChangesAsync();

        await transaction.CommitAsync();
        return Result<CreateOrderResultDto>.Success(new CreateOrderResultDto { Id = order.Id });
    }

    private void CreateOrderItemAsync(CoreOrder order)
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
                        Status = OrderItemStatus.Unprinted, // Trạng thái item ban đầu
                        OrderDetailId = detail.Id,
                    }
                );
            }
        }
        _dbContext.SaveChanges();
    }

    public async Task<Result<PagingResult<OrderBaseDto>>> FindAllOrder(FilterOrderDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(FindAllOrder),
            nameof(input),
            input
        );
        var listOrders = _dbContext
            .CoreOrders.Include(o => o.OrderDetails)
            .Where(e =>
                (
                    string.IsNullOrEmpty(input.Keyword)
                    || e.Name.ToLower().Contains(input.Keyword.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.Address)
                    || e.Address.ToLower().Contains(input.Address.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.City)
                    || e.City.ToLower().Contains(input.City.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.State)
                    || e.State.ToLower().Contains(input.State.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.PostalCode)
                    || e.PostalCode.ToLower().Contains(input.PostalCode.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.Country)
                    || e.Country.ToLower().Contains(input.Country.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.OrderNumber)
                    || e.OrderNumber.ToLower().Contains(input.OrderNumber.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.Tax) || e.Tax.ToLower().Contains(input.Tax.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.Phone)
                    || e.Phone.ToLower().Contains(input.Phone.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.Email)
                    || e.Email.ToLower().Contains(input.Email.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.Address2)
                    || e.Address2.ToLower().Contains(input.Address2.ToLower())
                )
                && (input.Status == null || e.Status == input.Status)
                && (
                    string.IsNullOrEmpty(input.Namespace)
                    || e.Namespace.ToLower().Contains(input.Namespace.ToLower())
                )
            )
            .Select(e => new OrderBaseDto
            {
                Id = e.Id,
                Name = e.Name,
                Address = e.Address,
                City = e.City ?? string.Empty,
                State = e.State ?? string.Empty,
                PostalCode = e.PostalCode ?? string.Empty,
                Country = e.Country ?? string.Empty,
                OrderNumber = e.OrderNumber,
                Status = e.Status,
                Tax = e.Tax ?? string.Empty,
                Phone = e.Phone ?? string.Empty,
                Email = e.Email ?? string.Empty,
                Address2 = e.Address2 ?? string.Empty,
                Namespace = e.Namespace ?? string.Empty,
            });
        int totalItems = await listOrders.CountAsync();
        var items = await listOrders.PagingAndSorting(input).ToListAsync();
        return Result<PagingResult<OrderBaseDto>>.Success(
            new PagingResult<OrderBaseDto> { TotalItems = totalItems, Items = items }
        );
    }

    public async Task<Result<PagingResult<OrderBaseDto>>> FindAllOrderPending(
        PagingRequestBaseDto input
    )
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(FindAllOrder),
            nameof(input),
            input
        );
        var listOrderPendings = _dbContext
            .CoreOrders.Where(e =>
                (
                    e.Status == OrderStatus.PendingImported
                    && (
                        string.IsNullOrEmpty(input.Keyword)
                        || e.Name.Contains(input.Keyword.ToLower())
                    )
                )
            )
            .Select(e => new OrderBaseDto
            {
                Id = e.Id,
                Namespace = e.Namespace,
                Tax = e.Tax,
                Phone = e.Phone,
                Email = e.Email,
                Address2 = e.Address2,
                Name = e.Name,
                Address = e.Address,
                City = e.City ?? string.Empty,
                State = e.State ?? string.Empty,
                PostalCode = e.PostalCode ?? string.Empty,
                Country = e.Country ?? string.Empty,
                OrderNumber = e.OrderNumber,
                BrandId = e.BrandId,
            });
        int totalItems = await listOrderPendings.CountAsync();
        var items = await listOrderPendings.PagingAndSorting(input).ToListAsync();
        return Result<PagingResult<OrderBaseDto>>.Success(
            new PagingResult<OrderBaseDto> { TotalItems = totalItems, Items = items }
        );
    }

    public async Task<Result<OrderDto>> FindById(int id)
    {
        _logger.LogInformation("{MethodName}: {Id} = {IdValue}", nameof(FindById), nameof(id), id);
        if (!await _dbContext.CoreOrders.AnyAsync(u => u.Id == id))
        {
            return Result<OrderDto>.Failure(
                CoreErrorCode.CoreOrderNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        var order = await _dbContext
            .CoreOrders.Include(o => o.OrderDetails)
            .Select(e => new OrderDto
            {
                Id = e.Id,
                Name = e.Name,
                Address = e.Address,
                City = e.City ?? string.Empty,
                State = e.State ?? string.Empty,
                PostalCode = e.PostalCode ?? string.Empty,
                Country = e.Country ?? string.Empty,
                OrderNumber = e.OrderNumber,
                Namespace = e.Namespace ?? string.Empty,
                Address2 = e.Address2 ?? string.Empty,
                BrandId = e.BrandId,
                Email = e.Email ?? string.Empty,
                Phone = e.Phone ?? string.Empty,
                Tax = e.Tax ?? string.Empty,
                Status = e.Status,
                Details = e.OrderDetails.Select(d => new OrderDetailBaseDto
                {
                    Id = d.Id,
                    OrderId = d.OrderId,
                    Type = d.Type,
                    Title = d.Title,
                    Size = d.Size,
                    SellerSku = d.SellerSku,
                    Color = d.Color,
                    Quantity = d.Quantity,
                    MockUpFront = d.MockUpFront,
                    MockUpBack = d.MockUpBack,
                    SaleDesignFront = d.SaleDesignFront,
                    SaleDesignBack = d.SaleDesignBack,
                    SaleDesignSleeves = d.SaleDesignSleeves,
                    SaleDesignHood = d.SaleDesignHood,
                }),
            })
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();
        if (order == null)
        {
            return Result<OrderDto>.Failure(
                CoreErrorCode.CoreOrderNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        return Result<OrderDto>.Success(order);
    }

    public async Task<Result<UpdateOrderResultDto>> UpdateOrder(UpdateOrderDto input)
    {
        _logger.LogInformation("{MethodName}: input = {@Input}", nameof(UpdateOrder), input);

        var order = await _dbContext.CoreOrders.FirstOrDefaultAsync(u => u.Id == input.Id);
        if (order == null)
        {
            return Result<UpdateOrderResultDto>.Failure(
                CoreErrorCode.CoreOrderNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        ;
        order.Id = input.Id;
        order.Name = input.Name;
        order.Address = input.Address;
        order.Address2 = input.Address2;
        order.Email = input.Email;
        order.Phone = input.Phone;
        order.Tax = input.Tax;
        order.City = input.City;
        order.State = input.State;
        order.PostalCode = input.PostalCode;
        order.Country = input.Country;
        order.OrderNumber = input.OrderNumber;
        order.BrandId = input.BrandId;
        order.Namespace = input.Namespace;
        await _dbContext.SaveChangesAsync();

        return Result<UpdateOrderResultDto>.Success(new UpdateOrderResultDto { Id = order.Id });
    }

    public async Task<Result> UpdateOrderItem()
    {
        throw new NotImplementedException();
    }

    public Task<Result> UpdateOrderItemNote()
    {
        throw new NotImplementedException();
    }

    public async Task<Result> ApproveDesignItem(int orderItemId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> ApproveOrder(int id)
    {
        _logger.LogInformation(
            "{MethodName}: {IdName} = {IdValue}",
            nameof(ApproveOrder),
            nameof(id),
            id
        );
        var order = await _dbContext.CoreOrders.FirstOrDefaultAsync(u => u.Id == id);
        if (order == null)
        {
            return Result.Failure(CoreErrorCode.CoreOrderNotFound, this.GetCurrentMethodInfo());
        }
        if (
            await _dbContext.CoreOrderDetails.AnyAsync(od =>
                od.OrderId == id
                && (
                    od.Status == OrderDetailStatus.Processing
                    || od.Status == OrderDetailStatus.Pending
                    || od.Status == OrderDetailStatus.Failed
                )
            )
        )
        {
            return Result.Failure(
                CoreErrorCode.CoreOrderDetailNotDone,
                this.GetCurrentMethodInfo()
            );
        }
        if (order.Status == OrderStatus.PendingImported)
        {
            order.Status = OrderStatus.PushedOrder;
            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }
        else
        {
            return Result.Failure(CoreErrorCode.CoreOrderPushed, this.GetCurrentMethodInfo());
        }
    }

    public async Task<Result> Delete(int id)
    {
        _logger.LogInformation("{MethodName}: id = {Id}", nameof(Delete), id);
        var order = await _dbContext
            .CoreOrders.Include(x => x.OrderDetails)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (order == null)
        {
            return Result.Failure(CoreErrorCode.CoreOrderNotFound, this.GetCurrentMethodInfo());
        }
        if (order.Status == OrderStatus.PendingImported)
        {
            _dbContext.CoreOrderDetails.RemoveRange(order.OrderDetails);

            _dbContext.CoreOrders.Remove(order);

            await _dbContext.SaveChangesAsync();
            return Result.Success();
        }
        else
        {
            return Result.Failure(CoreErrorCode.CoreOrderCantDelete, this.GetCurrentMethodInfo());
        }
    }
}
