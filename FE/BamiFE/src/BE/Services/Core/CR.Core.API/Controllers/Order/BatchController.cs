using CR.Core.ApplicationServices.OrderModule.Abstracts;
using CR.Core.Dtos.BatchModule;
using CR.Core.Dtos.Note;
using CR.Core.Dtos.OrderModule.Order;
using CR.DtoBase;
using CR.Utils.Net.Request;
using CR.WebAPIBase.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CR.Core.API.Controllers.Order;

/// <summary>
/// Quản lý lô sản xuất (cùng SKU), quản lý trạng thái lô
/// </summary>
[Authorize]
[Route("api/order/batch")]
[ApiController]
public class BatchController : ApiControllerBase
{
    private readonly IBatchService _batchService;

    public BatchController(ILogger<BatchController> logger, IBatchService batchService)
        : base(logger)
    {
        _batchService = batchService;
    }

    /// <summary>
    /// Tạo mới lô sản xuất
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("add")]
    public async Task<ApiResponse> CreateBatch(CreateBatchDto input)
    {
        try
        {
            return OkResult(await _batchService.Create(input));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Cập nhật thông tin lô sản xuất
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("update")]
    public async Task<ApiResponse> UpdateBatch(UpdateBatchDto input)
    {
        try
        {
            return OkResult(await _batchService.Update(input));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Xóa lô sản xuất
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("delete/{id}")]
    public async Task<ApiResponse> DeleteBatch(int id)
    {
        try
        {
            return OkResult(await _batchService.Delete(id));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Tìm kiếm lô sản xuất theo ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("find/{id}")]
    [ProducesResponseType(typeof(ApiResponse<BatchDto>), StatusCodes.Status200OK)]
    public async Task<ApiResponse> FindBatchById(int id)
    {
        try
        {
            return OkResult(await _batchService.FindById(id));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Lấy danh sách lô sản xuất
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("find-all")]
    [ProducesResponseType(typeof(ApiResponse<PagingResult<BatchDto>>), StatusCodes.Status200OK)]
    public async Task<ApiResponse> FindAllBatches(FilterBatchDto input)
    {
        try
        {
            return OkResult(await _batchService.FindAll(input));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Cập nhật trạng thái các mục trong lô
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("update-item-status")]
    public async Task<ApiResponse> UpdateBatchItemStatus(UpdateBatchItemStatusDto input)
    {
        try
        {
            return OkResult(await _batchService.UpdateItemStatus(input));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Lấy danh sách order items theo danh sách batchId
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("find-items-by-batch-ids")]
    [ProducesResponseType(typeof(ApiResponse<List<OrderItemDto>>), StatusCodes.Status200OK)]
    public async Task<ApiResponse> FindOrderItemByBatchIds(FindItemsByBatchDto input)
    {
        try
        {
            return OkResult(await _batchService.FindOrderItemByBatchIds(input));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }

    /// <summary>
    /// Cập nhật ghi chú cho lô
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("update-note")]
    public async Task<ApiResponse> UpdateNote(UpdateNoteDto input)
    {
        try
        {
            return OkResult(await _batchService.UpdateNote(input));
        }
        catch (Exception ex)
        {
            return OkException(ex);
        }
    }
}
