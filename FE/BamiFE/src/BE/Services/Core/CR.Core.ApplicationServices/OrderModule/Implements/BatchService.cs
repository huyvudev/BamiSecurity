using CR.ApplicationBase.Common;
using CR.Constants.Core.Order;
using CR.Constants.ErrorCodes;
using CR.Core.ApplicationServices.Common;
using CR.Core.ApplicationServices.OrderModule.Abstracts;
using CR.Core.Domain.Batch;
using CR.Core.Domain.Order;
using CR.Core.Dtos.BatchModule;
using CR.Core.Dtos.Note;
using CR.Core.Dtos.OrderModule.Order;
using CR.DtoBase;
using CR.Utils.DataUtils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.OrderModule.Implements;

public class BatchService : CoreServiceBase, IBatchService
{
    public BatchService(ILogger<BatchService> logger, IHttpContextAccessor httpContext)
        : base(logger, httpContext) { }

    //tạo lô theo danh sách item
    public async Task<Result> Create(CreateBatchDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(Create),
            nameof(input),
            input
        );
        // Lấy danh sách OrderItem và kiểm tra SKU
        var allOrderItems = await _dbContext
            .CoreOrderItems.Include(o => o.OrderDetail)
            .Where(o => input.Items.Select(item => item.OrderId).Contains(o.OrderId))
            .ToListAsync();
        List<CoreOrderItem> orderItems = [];
        foreach (var item in input.Items)
        {
            var addItem = allOrderItems.Find(e =>
                e.OrderId == item.OrderId && e.ItemIndex == item.ItemIndex && e.BatchId == null
            );
            if (addItem != null)
            {
                orderItems.Add(addItem);
            }
            else
            {
                return Result.Failure(
                    CoreErrorCode.CoreOrderItemInvalidOrInOtherBatch,
                    this.GetCurrentMethodInfo()
                );
            }
        }
        if (orderItems.Count == 0)
        {
            return Result.Failure(CoreErrorCode.CoreBatchMustNotEmpty, this.GetCurrentMethodInfo());
        }
        allOrderItems = allOrderItems
            .Where(ai =>
                orderItems.Any(oi =>
                    oi.OrderId == ai.OrderId && oi.ItemIndex == ai.ItemIndex && oi.BatchId == null
                )
            )
            .ToList();

        // Kiểm tra nếu tất cả các OrderItem có chung SKU
        var distinctSkus = allOrderItems
            .Where(oi => oi.OrderDetail != null)
            .Select(oi => oi.OrderDetail.SkuId)
            .Distinct()
            .ToList();

        if (distinctSkus.Count > 1)
        {
            return Result.Failure(
                CoreErrorCode.CoreOrderItemsDifferentSku,
                this.GetCurrentMethodInfo()
            );
        }
        var sku = await _dbContext
            .CoreSkus.Where(e => e.Id == allOrderItems[0].OrderDetail.SkuId)
            .FirstOrDefaultAsync();
        await _dbContext.CoreBatches.AddAsync(
            new CoreBatch
            {
                CreatorName = input.CreatorName,
                PartnerId = input.PartnerId,
                Priority = input.Priority,
                Status = BatchStatus.Pending,
                SkuId = allOrderItems[0].OrderDetail.SkuId,
                Sku = sku,
                OrderItems = orderItems,
            }
        );
        await _dbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> Delete(int id)
    {
        _logger.LogInformation("{MethodName}: {Id} = {IdValue}", nameof(Delete), nameof(id), id);
        var batch = await _dbContext.CoreBatches.Where(e => e.Id == id).FirstOrDefaultAsync();
        if (batch == null)
        {
            return Result.Failure(CoreErrorCode.CoreBatchNotFound, this.GetCurrentMethodInfo());
        }
        _dbContext.CoreBatches.Remove(batch);
        await _dbContext.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<PagingResult<BatchDto>>> FindAll(FilterBatchDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(FindAll),
            nameof(input),
            input
        );
        var batches = _dbContext
            .CoreBatches.Include(e => e.OrderItems)
            .Where(e =>
                (input.Status == null || e.Status == input.Status)
                && (input.Priority == null || e.Priority == input.Priority)
                && (string.IsNullOrEmpty(input.CreatorName) || e.CreatorName == input.CreatorName)
                && (input.CreateDateStart == null || e.CreatedDate >= input.CreateDateStart)
                && // Nếu chỉ nhập start
                (input.CreateDateEnd == null || e.CreatedDate <= input.CreateDateEnd)
                && // Nếu chỉ nhập end
                (
                    input.CreateDateStart == null && input.CreateDateEnd == null
                    || // Nếu nhập cả hai
                    (e.CreatedDate >= input.CreateDateStart && e.CreatedDate <= input.CreateDateEnd)
                )
                &&
                // Trường hợp Ngày in lô
                (input.PrintDateStart == null || e.PrintDate >= input.PrintDateStart)
                && (input.PrintDateEnd == null || e.PrintDate <= input.PrintDateEnd)
                && (
                    input.PrintDateStart == null && input.PrintDateEnd == null
                    || (e.PrintDate >= input.PrintDateStart && e.PrintDate <= input.PrintDateEnd)
                )
                &&
                // Trường hợp Ngày cắt
                (input.CutDateStart == null || e.CutDate >= input.CutDateStart)
                && (input.CutDateEnd == null || e.CutDate <= input.CutDateEnd)
                && (
                    input.CutDateStart == null && input.CutDateEnd == null
                    || (e.CutDate >= input.CutDateStart && e.CutDate <= input.CutDateEnd)
                )
                &&
                // Trường hợp Ngày khắc
                (input.EngravedDateStart == null || e.EngravedDate >= input.EngravedDateStart)
                && (input.EngravedDateEnd == null || e.EngravedDate <= input.EngravedDateEnd)
                && (
                    input.EngravedDateStart == null && input.EngravedDateEnd == null
                    || (
                        e.EngravedDate >= input.EngravedDateStart
                        && e.EngravedDate <= input.EngravedDateEnd
                    )
                )
                &&
                // Trường hợp Ngày hoàn thiện
                (input.FinishDateStart == null || e.FinishDate >= input.FinishDateStart)
                && (input.FinishDateEnd == null || e.FinishDate <= input.FinishDateEnd)
                && (
                    input.FinishDateStart == null && input.FinishDateEnd == null
                    || (
                        e.FinishDate >= input.FinishDateStart && e.FinishDate <= input.FinishDateEnd
                    )
                )
                && (input.SkuId == null || e.SkuId == input.SkuId)
                && (input.PartnerId == null || e.PartnerId == input.PartnerId)
                && (string.IsNullOrEmpty(input.Note) || e.Note == input.Note)
            )
            .Include(e => e.Partner)
            .Select(e => new BatchDto
            {
                Id = e.Id,
                CreatorName = e.CreatorName,
                Sku = e.Sku != null ? e.Sku.Code : null,
                OrderItemIds = e.OrderItems.Select(e => e.Id).ToList(),
                PartnerId = e.PartnerId,
                Priority = e.Priority,
                CreatedBy = e.CreatedBy,
                CreatedDate = e.CreatedDate,
                CutDate = e.CutDate,
                EngravedDate = e.EngravedDate,
                FinishDate = e.FinishDate,
                PrintDate = e.PrintDate,
                PartnerName = e.Partner.Name,
                Status = e.Status,
                Note = e.Note,
            });
        int totalItem = await batches.CountAsync();
        batches = batches.PagingAndSorting(input);
        return Result<PagingResult<BatchDto>>.Success(
            new PagingResult<BatchDto> { Items = batches, TotalItems = totalItem }
        );
    }

    //xem chi tiết thông tin lô
    public async Task<Result<FindBatchDto>> FindById(int id)
    {
        _logger.LogInformation("{MethodName}: {Id} = {IdValue}", nameof(Delete), nameof(id), id);
        var batch = await _dbContext
            .CoreBatches.Include(e => e.OrderItems)
            .ThenInclude(e => e.Order)
            .Include(e => e.OrderItems)
            .Include(e => e.OrderItems)
            .ThenInclude(e => e.Order)
            .ThenInclude(e => e.Brand)
            .Include(e => e.OrderItems)
            .ThenInclude(e => e.OrderDetail)
            .ThenInclude(e => e.Sku)
            .Include(e => e.Partner)
            .FirstOrDefaultAsync(e => e.Id == id);
        if (batch == null)
        {
            return Result<FindBatchDto>.Failure(
                CoreErrorCode.CoreBatchNotFound,
                this.GetCurrentMethodInfo()
            );
        }
        return Result<FindBatchDto>.Success(
            new FindBatchDto
            {
                Id = id,
                CreatorName = batch.CreatorName,
                Sku = batch.Sku?.Code,
                OrderItems = batch
                    .OrderItems.Select(e => new OrderItemDto
                    {
                        Id = e.Id,
                        ItemIndex = e.ItemIndex,
                        Status = e.Status,
                        Note = e.Note,
                        OrderDetailId = e.OrderDetailId,
                        OrderId = e.OrderId,
                        OrderStatus = e.Order.Status,
                        OrderDetailStatus = e.OrderDetail.Status,
                        OrderNumber = e.Order.OrderNumber,
                        Namespace = e.Order.Namespace,
                        IdSku = e.OrderDetail.SkuId,
                        Code = e.OrderDetail?.Sku?.Code,
                        IdBrand = e.Order.BrandId,
                        BrandName = e.Order?.Brand?.Name,
                        Size = e.OrderDetail?.Size,
                        Width = e.OrderDetail?.Width,
                        Length = e.OrderDetail?.Length,
                        ErrorMessage = e.OrderDetail?.ErrorMessage,
                        MockUpBack = e.OrderDetail?.MockUpBack,
                        MockUpFront = e.OrderDetail?.MockUpFront,
                        DesignBack = e.OrderDetail?.DesignBack,
                        DesignFront = e.OrderDetail?.DesignFront,
                        DesignHood = e.OrderDetail?.DesignHood,
                        DesignSleeves = e.OrderDetail?.DesignSleeves,
                        CreatedDate = e.CreatedDate,
                        State = e.Order?.State,
                        NumberItems = e.Order?.OrderItems.Count,                      
                    })
                    .ToList(),
                PartnerId = batch.PartnerId,
                Priority = batch.Priority,
                CreatedBy = batch.CreatedBy,
                CreatedDate = batch.CreatedDate,
                CutDate = batch.CutDate,
                EngravedDate = batch.EngravedDate,
                FinishDate = batch.FinishDate,
                PrintDate = batch.PrintDate,
                PartnerName = batch.Partner.Name,
                Status = batch.Status,
                Note = batch.Note,
            }
        );
    }

    public async Task<Result<List<BatchItemDto>>> FindOrderItemByBatchIds(FindItemsByBatchDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(FindOrderItemByBatchIds),
            nameof(input),
            input
        );
        var batches = await _dbContext
            .CoreBatches.Include(e => e.OrderItems)
            .Where(e => input.BatchIds.Contains(e.Id))
            .ToListAsync();
        // tìm lô không hợp lệ ( không có ID )
        var invalidBatchIds = input.BatchIds.Except(batches.Select(b => b.Id)).ToList();
        if (invalidBatchIds.Any())
        {
            return Result<List<BatchItemDto>>.Failure(
                CoreErrorCode.CoreBatchInvalid,
                this.GetCurrentMethodInfo(),
                listParam: GetInvalidBatch(invalidBatchIds)
            );
        }
        var result = new List<BatchItemDto>();
        foreach (var batch in batches)
        {
            var items = batch
                .OrderItems.Select(e => new BatchItemDto
                {
                    ItemIndex = e.ItemIndex,
                    OrderId = e.OrderId,
                })
                .ToList();
            result.AddRange(items);
        }
        return Result<List<BatchItemDto>>.Success(result);
    }

    private static string GetInvalidBatch(List<int> batchIds)
    {
        // Kiểm tra danh sách rỗng hoặc null
        if (batchIds == null || batchIds.Count == 0)
        {
            return "";
        }

        // Định dạng từng phần tử theo kiểu "B-Id lô {batchId}"
        var formattedBatchList = batchIds.Select(id => $"B-{id}");

        // Ghép các phần tử lại với dấu phẩy
        string invalidBatchList = string.Join(", ", formattedBatchList);

        // Tạo thông báo
        return $"{invalidBatchList}";
    }

    public async Task<Result> Update(UpdateBatchDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(Update),
            nameof(input),
            input
        );
        var batch = await _dbContext.CoreBatches.FirstOrDefaultAsync(e =>
            e.Id == input.Id && e.Status == BatchStatus.Pending
        );
        if (batch == null)
        {
            return Result.Failure(CoreErrorCode.CoreBatchNotFound, this.GetCurrentMethodInfo());
        }
        var orderIds = input.Items.Select(item => item.OrderId).Distinct().ToList();
        var allOrderItems = await _dbContext
            .CoreOrderItems.Include(o => o.OrderDetail)
            .Where(o => orderIds.Contains(o.OrderId))
            .ToListAsync();
        List<CoreOrderItem> orderItems = new List<CoreOrderItem>();
        foreach (var item in input.Items)
        {
            var addItem = allOrderItems.Find(e =>
                e.OrderId == item.OrderId && e.ItemIndex == item.ItemIndex
            );
            if (addItem != null && (addItem.BatchId == input.Id || addItem.BatchId == null))
            {
                orderItems.Add(addItem);
            }
            else
            {
                return Result.Failure(
                    CoreErrorCode.CoreOrderItemInvalidOrInOtherBatch,
                    this.GetCurrentMethodInfo(),
                    listParam: GetItemTemplate(item.OrderId, item.ItemIndex)
                );
            }
        }
        if (orderItems.Count == 0)
        {
            return Result.Failure(CoreErrorCode.CoreBatchMustNotEmpty, this.GetCurrentMethodInfo());
        }
        allOrderItems = allOrderItems
            .Where(ai =>
                orderItems.Any(oi => oi.OrderId == ai.OrderId && oi.ItemIndex == ai.ItemIndex)
            )
            .ToList();

        // Kiểm tra nếu tất cả các OrderItem có chung SKU
        var distinctSkus = allOrderItems
            .Where(oi => oi.OrderDetail != null)
            .Select(oi => oi.OrderDetail.SkuId)
            .Distinct()
            .ToList();

        if (distinctSkus.Count > 1)
        {
            return Result.Failure(
                CoreErrorCode.CoreOrderItemsDifferentSku,
                this.GetCurrentMethodInfo()
            );
        }
        var sku = await _dbContext
            .CoreSkus.Where(e => e.Id == allOrderItems[0].OrderDetail.SkuId)
            .FirstOrDefaultAsync();
        batch.CreatorName = input.CreatorName;
        batch.PartnerId = input.PartnerId;
        batch.Priority = input.Priority;
        batch.OrderItems = orderItems;
        batch.SkuId = allOrderItems[0].OrderDetail.SkuId;
        batch.Sku = sku;
        await _dbContext.SaveChangesAsync();
        return Result.Success();
    }

    //tạo qr lô

    //cập nhật trạng thái item trong lô
    public async Task<Result> UpdateItemStatus(UpdateBatchItemStatusDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(UpdateItemStatus),
            nameof(input),
            input
        );

        // Lấy tất cả items cần update
        var allItems = await _dbContext
            .CoreOrderItems.Where(o => input.Items.Select(item => item.OrderId).Contains(o.OrderId))
            .ToListAsync();

        // Kiểm tra xem tất cả items có tồn tại không
        var orderItems = new List<CoreOrderItem>();
        foreach (var inputItem in input.Items)
        {
            var item = allItems.Find(e =>
                e.OrderId == inputItem.OrderId && e.ItemIndex == inputItem.ItemIndex
            );
            if (item == null)
            {
                return Result.Failure(
                    CoreErrorCode.CoreOrderItemNotFound,
                    this.GetCurrentMethodInfo()
                );
            }
            // Kiểm tra status không được phép revert
            if (item.Status > input.ItemStatus)
            {
                return Result.Failure(
                    CoreErrorCode.CoreOrderItemStatusCannotRevert,
                    this.GetCurrentMethodInfo(),
                    listParam: GetItemTemplate(item.OrderId, item.ItemIndex)
                );
            }
            orderItems.Add(item);
        }

        // Lấy thông tin các batch liên quan
        var batchIds = orderItems.Select(i => i.BatchId).Distinct();
        var batches = await _dbContext
            .CoreBatches.Include(e => e.OrderItems)
            .Where(e => batchIds.Contains(e.Id))
            .ToListAsync();

        // Kiểm tra xem tất cả items có thuộc batch không
        if (orderItems.Any(item => item.BatchId == null || !batches.Any(b => b.Id == item.BatchId)))
        {
            var item = orderItems.FirstOrDefault(e => e.BatchId == null);
            return Result.Failure(
                CoreErrorCode.CoreOrderItemNotInBatch,
                this.GetCurrentMethodInfo(),
                listParam: GetItemTemplate(item!.OrderId, item.ItemIndex)
            );
        }

        // Update status cho tất cả items
        foreach (var inputItem in input.Items)
        {
            var item = orderItems.Find(e =>
                e.OrderId == inputItem.OrderId && e.ItemIndex == inputItem.ItemIndex
            );
            if (item != null)
            {
                item.Status = input.ItemStatus;
            }
        }

        // Update status cho các batch
        foreach (var batch in batches)
        {
            UpdateBatchStatus(batch);
        }

        await _dbContext.SaveChangesAsync();
        return Result.Success();
    }

    private static void UpdateBatchStatus(CoreBatch batch)
    {
        // tất cả item đã QC, chuyển trạng thái lô sang hoàn thành
        if (!batch.OrderItems.Any(e => e.Status != OrderItemStatus.QualityChecked))
        {
            batch.Status = BatchStatus.Finished;
            batch.FinishDate = DateTimeUtils.GetDate();
        }
        // nếu ít nhất một item đã QC, chuyển trạng thái lô sang QC
        else if (batch.OrderItems.Any(e => e.Status == OrderItemStatus.QualityChecked))
        {
            batch.Status = BatchStatus.QualityCheck;
        }
        // nếu tất cả item đã khắc, chuyển trạng thái lô sang đã khắc
        else if (!batch.OrderItems.Any(e => e.Status != OrderItemStatus.Engraved))
        {
            batch.Status = BatchStatus.Engraved;
            batch.EngravedDate = DateTimeUtils.GetDate();
        }
        //nếu tất cả item đã cắt, chuyển trạng thái lô sang đã cắt
        else if (!batch.OrderItems.Any(e => e.Status != OrderItemStatus.Cut))
        {
            batch.Status = BatchStatus.Cut;
            batch.CutDate = DateTimeUtils.GetDate();
        }
        //nếu tất cả item đã in, chuyển trạng thái lô sang đã in xong
        else if (!batch.OrderItems.Any(e => e.Status != OrderItemStatus.Printed))
        {
            batch.Status = BatchStatus.Printed;
            batch.PrintDate = DateTimeUtils.GetDate();
        }
    }

    public async Task<Result> UpdateNote(UpdateNoteDto input)
    {
        _logger.LogInformation(
            "{MethodName}: {InputName} = {@InputValue}",
            nameof(UpdateNote),
            nameof(input),
            input
        );
        var batch = await _dbContext.CoreBatches.Where(e => e.Id == input.Id).FirstOrDefaultAsync();
        if (batch == null)
        {
            return Result.Failure(CoreErrorCode.CoreBatchNotFound, this.GetCurrentMethodInfo());
        }
        batch.Note = input.Note;
        await _dbContext.SaveChangesAsync();
        return Result.Success();
    }
}
