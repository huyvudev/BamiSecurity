using CR.ApplicationBase.Common;
using CR.Constants.Core.Order;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.Common;
using CR.Core.ApplicationServices.OrderModule.Abstracts;
using CR.Core.Domain.Order;
using CR.Core.Dtos.OrderModule.OrderDetail;
using CR.DtoBase;
using CR.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.OrderModule.Implements;

public class OrderDetailService : CoreServiceBase, IOrderDetailService
{
    public OrderDetailService(ILogger<OrderDetailService> logger, IHttpContextAccessor httpContext)
        : base(logger, httpContext) { }

    public async Task<Result<PagingResult<OrderDetailDoneDto>>> FindAllOrderDetailDone(
        PagingRequestBaseDto input
    )
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(FindAllOrderDetailDone),
            nameof(input),
            input
        );
        var listOrderDetails = _dbContext
            .CoreOrderDetails.Include(x => x.Order)
            .Where(e =>
                e.Status == OrderDetailStatus.Done
                && e.Order.Status == OrderStatus.PushedOrder
                && (
                    string.IsNullOrEmpty(input.Keyword)
                    || e.Type.ToLower().Contains(input.Keyword.ToLower())
                )
            )
            .OrderByDescending(x => x.OrderId)
            .Select(e => new OrderDetailDoneDto
            {
                Id = e.Id,
                OrderId = e.OrderId,
                Type = e.Type,
                Title = e.Title,
                Size = e.Size,
                SellerSku = e.SellerSku,
                Color = e.Color,
                Quantity = e.Quantity,
                Width = e.Width,
                Length = e.Length,
                SkuId = e.SkuId,
                Code = e.Sku.Code,
                ErrorMessage = e.ErrorMessage,
                SaleDesignBack = e.SaleDesignBack,
                SaleDesignFront = e.SaleDesignFront,
                SaleDesignHood = e.SaleDesignHood,
                SaleDesignSleeves = e.SaleDesignSleeves,
                Status = e.Status,
                MockUpBack = e.MockUpBack,
                MockUpFront = e.MockUpFront,
                DesignFront = e.DesignFront,
                DesignBack = e.DesignBack,
                DesignHood = e.DesignHood,
                DesignSleeves = e.DesignSleeves,
            });
        int totalItems = await listOrderDetails.CountAsync();
        var items = await listOrderDetails.PagingAndSorting(input).ToListAsync();
        return Result<PagingResult<OrderDetailDoneDto>>.Success(
            new PagingResult<OrderDetailDoneDto> { TotalItems = totalItems, Items = items }
        );
    }

    public async Task<Result<PagingResult<OrderDetailDoneDto>>> FindAllOrderDetail(
        FilterOrderDetailDto input
    )
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(FindAllOrderDetail),
            nameof(input),
            input
        );
        var listOrderDetails = _dbContext
            .CoreOrderDetails.OrderByDescending(x => x.OrderId)
            .Where(e =>
                e.Order.Status == OrderStatus.PushedOrder
                && (
                    string.IsNullOrEmpty(input.Keyword)
                    || e.Type.ToLower().Contains(input.Keyword.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.Title)
                    || e.Title.ToLower().Contains(input.Title.ToLower())
                )
                && (input.OrderId == null || e.OrderId == input.OrderId)
                && (
                    string.IsNullOrEmpty(input.Size)
                    || e.Size.ToLower().Contains(input.Size.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.SellerSku)
                    || e.SellerSku.ToLower().Contains(input.SellerSku.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.Color)
                    || e.Color.ToLower().Contains(input.Color.ToLower())
                )
                && (input.Quantity == null || e.Quantity == input.Quantity)
                && (input.Status == null || e.Status == input.Status)
                && (
                    string.IsNullOrEmpty(input.Code)
                    || e.Sku.Code.ToLower().Contains(input.Code.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.ErrorMessage)
                    || e.ErrorMessage.ToLower().Contains(input.ErrorMessage.ToLower())
                )
                && (input.Width == null || e.Width == input.Width)
                && (input.Length == null || e.Length == input.Length)
                && (input.SkuId == null || e.SkuId == input.SkuId)
                && (
                    string.IsNullOrEmpty(input.DesignFront)
                    || e.DesignFront.ToLower().Contains(input.DesignFront.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.DesignBack)
                    || e.DesignBack.ToLower().Contains(input.DesignBack.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.DesignSleeves)
                    || e.DesignSleeves.ToLower().Contains(input.DesignSleeves.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.DesignHood)
                    || e.DesignHood.ToLower().Contains(input.DesignHood.ToLower())
                )
            )
            .Select(e => new OrderDetailDoneDto
            {
                Id = e.Id,
                OrderId = e.OrderId,
                Type = e.Type,
                Title = e.Title,
                Size = e.Size,
                SellerSku = e.SellerSku,
                Color = e.Color,
                Quantity = e.Quantity,
                Width = e.Width,
                Length = e.Length,
                SkuId = e.SkuId,
                Code = e.Sku.Code,
                ErrorMessage = e.ErrorMessage,
                SaleDesignBack = e.SaleDesignBack,
                SaleDesignFront = e.SaleDesignFront,
                SaleDesignHood = e.SaleDesignHood,
                SaleDesignSleeves = e.SaleDesignSleeves,
                Status = e.Status,
                MockUpBack = e.MockUpBack,
                MockUpFront = e.MockUpFront,
                DesignFront = e.DesignFront,
                DesignBack = e.DesignBack,
                DesignHood = e.DesignHood,
                DesignSleeves = e.DesignSleeves,
            });
        int totalItems = await listOrderDetails.CountAsync();
        var items = await listOrderDetails.PagingAndSorting(input).ToListAsync();
        return Result<PagingResult<OrderDetailDoneDto>>.Success(
            new PagingResult<OrderDetailDoneDto> { TotalItems = totalItems, Items = items }
        );
    }

    public async Task<Result<UpdateOrderDetailResultDto>> UpdateOrderDetail(
        UpdateOrderDetailBaseDto input
    )
    {
        _logger.LogInformation("{MethodName}: input = {@Input}", nameof(UpdateOrderDetail), input);
        var orderDetail = await _dbContext.CoreOrderDetails.FirstOrDefaultAsync(u =>
            u.Id == input.Id
        );
        if (orderDetail == null)
        {
            return Result<UpdateOrderDetailResultDto>.Failure(
                CoreErrorCode.CoreOrderDetailNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        if (orderDetail.Status == OrderDetailStatus.Approved)
        {
            return Result<UpdateOrderDetailResultDto>.Failure(
                CoreErrorCode.CoreOrderDetailApproved,
                this.GetCurrentMethodInfo()
            );
        }
        orderDetail.Id = input.Id;
        orderDetail.OrderId = input.OrderId;
        orderDetail.Type = input.Type;
        orderDetail.Title = input.Title;
        orderDetail.Size = input.Size;
        orderDetail.SellerSku = input.SellerSku;
        orderDetail.Color = input.Color;
        orderDetail.Quantity = input.Quantity;
        orderDetail.ErrorMessage = input.ErrorMessage;
        orderDetail.MockUpBack = input.MockUpBack;
        orderDetail.MockUpFront = input.MockUpFront;
        orderDetail.SaleDesignFront = input.SaleDesignFront;
        orderDetail.SaleDesignBack = input.SaleDesignBack;
        orderDetail.SaleDesignHood = input.SaleDesignHood;
        orderDetail.SaleDesignSleeves = input.SaleDesignSleeves;
        await _dbContext.SaveChangesAsync();

        return Result<UpdateOrderDetailResultDto>.Success(
            new UpdateOrderDetailResultDto { Id = orderDetail.Id }
        );
    }

    public async Task<Result<UpdateOrderDetailResultDto>> UpdateDetailDone(
        UpdateBasicInformationForOrderDetailDto input
    )
    {
        _logger.LogInformation("{MethodName}: input = {@Input}", nameof(UpdateDetailDone), input);
        var orderDetail = await _dbContext.CoreOrderDetails.FirstOrDefaultAsync(u =>
            u.Id == input.Id
        );
        if (orderDetail == null)
        {
            return Result<UpdateOrderDetailResultDto>.Failure(
                CoreErrorCode.CoreOrderDetailNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        //if (orderDetail.Status == OrderDetailStatus.Approved)
        //{
        //    return Result<UpdateOrderDetailResultDto>.Failure(
        //        CoreErrorCode.CoreOrderDetailApproved,
        //        this.GetCurrentMethodInfo()
        //    );
        //}
        //orderDetail.Id = input.Id;
        //orderDetail.OrderId = input.OrderId;

        orderDetail.Type = input.Type;
        orderDetail.Title = input.Title;
        orderDetail.Size = input.Size;
        orderDetail.SellerSku = input.SellerSku;
        orderDetail.Color = input.Color;
        orderDetail.Quantity = input.Quantity;

        orderDetail.SkuId = input.SkuId;
        orderDetail.Length = input.Length;
        orderDetail.Width = input.Width;
        orderDetail.ErrorMessage = input.ErrorMessage;

        orderDetail.MockUpBack = input.MockUpBack;
        orderDetail.MockUpFront = input.MockUpFront;

        orderDetail.DesignFront = input.DesignFront;
        orderDetail.DesignBack = input.DesignBack;
        orderDetail.DesignHood = input.DesignHood;
        orderDetail.DesignSleeves = input.DesignSleeves;
        orderDetail.SaleDesignBack = input.SaleDesignBack;
        orderDetail.SaleDesignSleeves = input.SaleDesignSleeves;
        orderDetail.SaleDesignHood = input.SaleDesignHood;
        orderDetail.SaleDesignFront = input.SaleDesignFront;

        await _dbContext.SaveChangesAsync();

        return Result<UpdateOrderDetailResultDto>.Success(
            new UpdateOrderDetailResultDto { Id = orderDetail.Id }
        );
    }

    public async Task<Result<OrderDetailDoneDto>> FindByIdDetailDone(int id)
    {
        _logger.LogInformation(
            "{MethodName}: {Id} = {IdValue}",
            nameof(FindByIdDetailDone),
            nameof(id),
            id
        );
        if (!await _dbContext.CoreOrderDetails.AnyAsync(u => u.Id == id))
        {
            return Result<OrderDetailDoneDto>.Failure(
                CoreErrorCode.CoreOrderDetailNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        var orderDetail = await _dbContext
            .CoreOrderDetails.Select(e => new OrderDetailDoneDto
            {
                Id = e.Id,
                OrderId = e.OrderId,
                Type = e.Type,
                Title = e.Title,
                Size = e.Size,
                SellerSku = e.SellerSku,
                Color = e.Color,
                Quantity = e.Quantity,
                Width = e.Width,
                Length = e.Length,
                SkuId = e.SkuId,
                Code = e.Sku.Code,
                ErrorMessage = e.ErrorMessage,
                SaleDesignBack = e.SaleDesignBack,
                SaleDesignFront = e.SaleDesignFront,
                SaleDesignHood = e.SaleDesignHood,
                SaleDesignSleeves = e.SaleDesignSleeves,
                Status = e.Status,
                MockUpBack = e.MockUpBack,
                MockUpFront = e.MockUpFront,
                DesignFront = e.DesignFront,
                DesignBack = e.DesignBack,
                DesignHood = e.DesignHood,
                DesignSleeves = e.DesignSleeves,
            })
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
        return Result<OrderDetailDoneDto>.Success(orderDetail);
    }

    public async Task<Result<ApproveOrderDetailResultDto>> ApproveOrderDetail(int id)
    {
        _logger.LogInformation(
            "{MethodName}: {Id} = {IdValue}",
            nameof(ApproveOrderDetail),
            nameof(id),
            id
        );

        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        var orderDetail = await _dbContext.CoreOrderDetails.FirstOrDefaultAsync(u => u.Id == id);
        if (orderDetail == null || orderDetail.Status != OrderDetailStatus.Done)
        {
            return Result<ApproveOrderDetailResultDto>.Failure(
                CoreErrorCode.CoreOrderDetailNotFound,
                this.GetCurrentMethodInfo()
            );
        }

        if (orderDetail.Width == null || orderDetail.Length == null || orderDetail.SkuId == null)
        {
            return Result<ApproveOrderDetailResultDto>.Failure(
                CoreErrorCode.CoreLackOfBasicInformationForOrderDetail,
                this.GetCurrentMethodInfo()
            );
        }

        orderDetail.Status = OrderDetailStatus.Approved;
        await _dbContext.SaveChangesAsync();

        var maxItemIndex =
            await _dbContext
                .CoreOrderItems.Where(e => e.OrderId == orderDetail.OrderId)
                .MaxAsync(e => (int?)e.ItemIndex) ?? 0;

        for (int i = 1; i <= orderDetail.Quantity; i++)
        {
            _dbContext.CoreOrderItems.Add(
                new CoreOrderItem
                {
                    ItemIndex = ++maxItemIndex,
                    OrderId = orderDetail.OrderId,
                    Status = OrderItemStatus.Unprinted,
                    OrderDetailId = orderDetail.Id,
                }
            );
        }

        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        // Trả kết quả thành công
        return Result<ApproveOrderDetailResultDto>.Success(
            new ApproveOrderDetailResultDto { Id = orderDetail.Id }
        );
    }

    public async Task<Result> Delete(int id)
    {
        _logger.LogInformation("{MethodName}: {Id} = {IdValue}", nameof(Delete), nameof(id), id);
        var orderDetail = await _dbContext.CoreOrderDetails.FirstOrDefaultAsync(u => u.Id == id);
        if (orderDetail == null)
        {
            return Result.Failure(
                CoreErrorCode.CoreOrderDetailNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        if (orderDetail.Status == OrderDetailStatus.Approved)
        {
            return Result.Failure(
                CoreErrorCode.CoreOrderDetailApproved,
                this.GetCurrentMethodInfo()
            );
        }
        _dbContext.CoreOrderDetails.Remove(orderDetail);
        await _dbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<CreateOrderDetailResultDto>> Create(AddOrderDetailDto input)
    {
        _logger.LogInformation("{MethodName}: input = {@Input}", nameof(Create), input);
        var order = await _dbContext.CoreOrders.FirstOrDefaultAsync(e => e.Id == input.OrderId);
        if (order == null)
        {
            return Result<CreateOrderDetailResultDto>.Failure(
                CoreErrorCode.CoreOrderNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        var newDetail = new CoreOrderDetail
        {
            OrderId = input.OrderId,
            Type = input.Type,
            Title = input.Title,
            Size = input.Size,
            SellerSku = input.SellerSku,
            Color = input.Color,
            Quantity = input.Quantity,
            MockUpBack = input.MockUpBack,
            MockUpFront = input.MockUpFront,
            SaleDesignBack = input.SaleDesignBack,
            SaleDesignFront = input.SaleDesignFront,
            SaleDesignHood = input.SaleDesignHood,
            SaleDesignSleeves = input.SaleDesignSleeves,
            Status = OrderDetailStatus.Done,
        };
        var addDetail = _dbContext.Add(newDetail).Entity;
        await _dbContext.SaveChangesAsync();
        return Result<CreateOrderDetailResultDto>.Success(
            new CreateOrderDetailResultDto { Id = addDetail.Id }
        );
    }
}
