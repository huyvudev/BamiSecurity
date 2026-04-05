using CR.Core.ApplicationServices.OrderModule.Abstracts;
using CR.Core.Infrastructure.Persistence;
using CR.InfrastructureBase.LoadFile;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.OrderModule.Implements;

public class HandleOrderService : IHandleOrderService
{
    private readonly ILogger _logger;
    private readonly CoreDbContext _dbContext;
    private readonly IImageLoader _imageLoader;

    public HandleOrderService(
        ILogger<HandleOrderService> logger,
        CoreDbContext dbContext,
        IImageLoader imageLoader
    )
    {
        _logger = logger;
        _dbContext = dbContext;
        _imageLoader = imageLoader;
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task HandleOrder(int orderId)
    {
        var order = await _dbContext
            .CoreOrders.Include(o => o.OrderDetails)
            .AsSplitQuery()
            .FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
        {
            _logger.LogError("Order not found: {OrderId}", orderId);
            return;
        }

        foreach (var detail in order.OrderDetails)
        {
            //load ảnh về
            var listTask = new List<Task>();
            var mockUpFrontTask = _imageLoader.LoadImageFromUrlAsync(detail.MockUpFront);
            listTask.Add(mockUpFrontTask);
            Task mockUpBackTask,
                designFrontTask,
                designBackTask,
                designSleevesTask,
                designHoodTask;
            if (!string.IsNullOrEmpty(detail.MockUpBack))
            {
                mockUpBackTask = _imageLoader.LoadImageFromUrlAsync(detail.MockUpBack);
                listTask.Add(mockUpBackTask);
            }
            if (!string.IsNullOrEmpty(detail.DesignFront))
            {
                designFrontTask = _imageLoader.LoadImageFromUrlAsync(detail.DesignFront);
                listTask.Add(designFrontTask);
            }
            if (!string.IsNullOrEmpty(detail.DesignBack))
            {
                designBackTask = _imageLoader.LoadImageFromUrlAsync(detail.DesignBack);
                listTask.Add(designBackTask);
            }
            if (!string.IsNullOrEmpty(detail.DesignSleeves))
            {
                designSleevesTask = _imageLoader.LoadImageFromUrlAsync(detail.DesignSleeves);
                listTask.Add(designSleevesTask);
            }
            if (!string.IsNullOrEmpty(detail.DesignHood))
            {
                designHoodTask = _imageLoader.LoadImageFromUrlAsync(detail.DesignHood);
                listTask.Add(designHoodTask);
            }

            await Task.WhenAll(listTask);

            //upload ảnh lên s3 và cập nhật lại link
        }
    }
}
