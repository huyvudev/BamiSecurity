using CR.ApplicationBase.Common;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.Common;
using CR.Core.ApplicationServices.ItemModule.Abstracts;
using CR.Core.Dtos.Note;
using CR.Core.Dtos.OrderModule.Item;
using CR.Core.Dtos.OrderModule.Order;
using CR.DtoBase;
using CR.Utils.DataUtils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.ItemModule.Implements;

public class OrderItemService : CoreServiceBase, IOrderItemService
{
    public OrderItemService(
        ILogger<OrderItemService> logger,
        IWebHostEnvironment environment,
        IHttpContextAccessor httpContext
    )
        : base(logger, httpContext) { }

    public async Task<Result<PagingResult<OrderItemDto>>> FindAllOrderItem(FilterOrderItemDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(FindAllOrderItem),
            nameof(input),
            input
        );

        var query = _dbContext
            .CoreOrderItems.Include(oi => oi.OrderDetail)
            .ThenInclude(s => s.Sku)
            .Include(oi => oi.Order)
            .ThenInclude(b => b.Brand)
            .Where(oi =>
                (
                    string.IsNullOrEmpty(input.Keyword)
                    || (oi.Note != null && oi.Note.ToLower().Contains(input.Keyword.ToLower()))
                )
                && (
                    string.IsNullOrEmpty(input.NameSpace)
                    || oi.Order.Namespace.ToLower().Contains(input.NameSpace.ToLower())
                )
                && (
                    string.IsNullOrEmpty(input.OrderNumber)
                    || oi.Order.OrderNumber.ToLower().Contains(input.OrderNumber.ToLower())
                )
                && (
                    (input.StartDate == null && input.EndDate == null)
                    || (
                        input.StartDate != null
                        && input.EndDate == null
                        && oi.CreatedDate >= input.StartDate.Value.Date
                    )
                    || (
                        input.StartDate == null
                        && input.EndDate != null
                        && oi.CreatedDate < input.EndDate.Value.Date.AddDays(1)
                    )
                    || (
                        input.StartDate != null
                        && input.EndDate != null
                        && oi.CreatedDate >= input.StartDate.Value.Date
                        && oi.CreatedDate < input.EndDate.Value.Date.AddDays(1)
                    )
                )
                && (
                    input.IdBrand == null
                    || (oi.Order.Brand != null && oi.Order.Brand.Id == input.IdBrand)
                )
                && (
                    input.IdSku == null
                    || (oi.OrderDetail.Sku != null && oi.OrderDetail.Sku.Id == input.IdSku)
                )
                && (input.Status == null || oi.Status == input.Status)
                && (input.ItemIndex == null || oi.ItemIndex == input.ItemIndex)
                && (input.OrderDetailId == null || oi.OrderDetailId == input.OrderDetailId)
                && (input.OrderId == null || oi.OrderId == input.OrderId)
            )
            .OrderByDescending(oi => oi.ItemIndex)
            .ThenByDescending(oi => oi.CreatedDate)
            .Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                OrderDetailId = oi.OrderDetailId,
                ItemIndex = oi.ItemIndex,
                Status = oi.Status,
                Note = oi.Note,
                OrderId = oi.OrderId,
                OrderStatus = oi.Order.Status,
                OrderDetailStatus = oi.OrderDetail.Status,
                OrderNumber = oi.Order.OrderNumber,
                Namespace = oi.Order.Namespace,
                IdSku = oi.OrderDetail.Sku != null ? oi.OrderDetail.Sku.Id : null,
                Code = oi.OrderDetail.Sku != null ? oi.OrderDetail.Sku.Code : null,
                IdBrand = oi.Order.Brand != null ? oi.Order.Brand.Id : null,
                BrandName = oi.Order.Brand != null ? oi.Order.Brand.Name : null,
                Size = oi.OrderDetail.Size,
                Width = oi.OrderDetail.Width,
                Length = oi.OrderDetail.Length,
                ErrorMessage = oi.OrderDetail.ErrorMessage,
                MockUpFront = oi.OrderDetail.MockUpFront ?? string.Empty,
                MockUpBack = oi.OrderDetail.MockUpBack,
                DesignFront = oi.OrderDetail.DesignFront,
                DesignBack = oi.OrderDetail.DesignBack,
                DesignSleeves = oi.OrderDetail.DesignSleeves,
                DesignHood = oi.OrderDetail.DesignHood,
                CreatedDate = oi.CreatedDate,
            });

        var totalItems = await query.CountAsync();
        var items = await query.PagingAndSorting(input).ToListAsync();

        return Result<PagingResult<OrderItemDto>>.Success(
            new PagingResult<OrderItemDto> { TotalItems = totalItems, Items = items }
        );
    }

    public async Task<Result<OrderItemDto>> FindOrderItemById(int orderItemId)
    {
        _logger.LogInformation(
            "{MethodName}: {OrderItemIdName} = {OrderItemIdValue}",
            nameof(FindOrderItemById),
            nameof(orderItemId),
            orderItemId
        );
        var item = await _dbContext
            .CoreOrderItems.Include(oi => oi.OrderDetail)
            .ThenInclude(oi => oi.Sku)
            .Include(oi => oi.Order)
            .Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                OrderDetailId = oi.OrderDetailId,
                ItemIndex = oi.ItemIndex,
                Status = oi.Status,
                Note = oi.Note ?? string.Empty,
                OrderId = oi.OrderId,
                OrderStatus = oi.Order.Status,
                OrderDetailStatus = oi.OrderDetail.Status,
                OrderNumber = oi.Order.OrderNumber,
                Namespace = oi.Order.Namespace,
                Code = oi.OrderDetail.Sku != null ? oi.OrderDetail.Sku.Code : string.Empty,
                Size = oi.OrderDetail.Size ?? string.Empty,
                Width = oi.OrderDetail.Width,
                Length = oi.OrderDetail.Length,
                ErrorMessage = oi.OrderDetail.ErrorMessage ?? string.Empty,
                MockUpFront = oi.OrderDetail.MockUpFront ?? string.Empty,
                MockUpBack = oi.OrderDetail.MockUpBack ?? string.Empty,
                DesignFront = oi.OrderDetail.DesignFront ?? string.Empty,
                DesignBack = oi.OrderDetail.DesignBack ?? string.Empty,
                DesignSleeves = oi.OrderDetail.DesignSleeves ?? string.Empty,
                DesignHood = oi.OrderDetail.DesignHood ?? string.Empty,
                CreatedDate = oi.CreatedDate,
            })
            .Where(oi => oi.Id == orderItemId)
            .FirstOrDefaultAsync();
        if (item == null)
        {
            return Result<OrderItemDto>.Failure(
                CoreErrorCode.CoreOrderItemNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        return Result<OrderItemDto>.Success(item);
    }

    public async Task<Result<OrderItemDto>> FindOrderItemByTemplate(FindOrderItemDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(FindOrderItemByTemplate),
            nameof(input),
            input
        );
        var item = await _dbContext
            .CoreOrderItems.Include(oi => oi.OrderDetail)
            .ThenInclude(oi => oi.Sku)
            .Include(oi => oi.Order)
            .Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                OrderDetailId = oi.OrderDetailId,
                ItemIndex = oi.ItemIndex,
                Status = oi.Status,
                Note = oi.Note ?? string.Empty,
                OrderId = oi.OrderId,
                OrderStatus = oi.Order.Status,
                OrderDetailStatus = oi.OrderDetail.Status,
                OrderNumber = oi.Order.OrderNumber,
                Namespace = oi.Order.Namespace,
                Code = oi.OrderDetail.Sku != null ? oi.OrderDetail.Sku.Code : string.Empty,
                Size = oi.OrderDetail.Size ?? string.Empty,
                Width = oi.OrderDetail.Width,
                Length = oi.OrderDetail.Length,
                ErrorMessage = oi.OrderDetail.ErrorMessage ?? string.Empty,
                MockUpFront = oi.OrderDetail.MockUpFront ?? string.Empty,
                MockUpBack = oi.OrderDetail.MockUpBack ?? string.Empty,
                DesignFront = oi.OrderDetail.DesignFront ?? string.Empty,
                DesignBack = oi.OrderDetail.DesignBack ?? string.Empty,
                DesignSleeves = oi.OrderDetail.DesignSleeves ?? string.Empty,
                DesignHood = oi.OrderDetail.DesignHood ?? string.Empty,
                CreatedDate = oi.CreatedDate,
            })
            .Where(oi => oi.ItemIndex == input.ItemIndex && oi.OrderId == input.OrderId)
            .FirstOrDefaultAsync();
        if (item == null)
        {
            return Result<OrderItemDto>.Failure(
                CoreErrorCode.CoreOrderItemNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        return Result<OrderItemDto>.Success(item);
    }

    public async Task<Result> UpdateNote(UpdateNoteDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(UpdateNote),
            nameof(input),
            input
        );
        var item = await _dbContext
            .CoreOrderItems.Where(e => e.Id == input.Id)
            .FirstOrDefaultAsync();
        if (item == null)
        {
            return Result.Failure(CoreErrorCode.CoreOrderItemNotFound, this.GetCurrentMethodInfo());
        }
        item.Note = input.Note;
        await _dbContext.SaveChangesAsync();
        return Result.Success();
    }
}
