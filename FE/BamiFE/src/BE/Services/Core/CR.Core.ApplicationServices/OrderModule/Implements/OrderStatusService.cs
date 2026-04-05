using CR.Constants.Core.Order;
using CR.Core.ApplicationServices.Common;
using CR.Core.ApplicationServices.OrderModule.Abstracts;
using CR.DtoBase;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CR.Core.ApplicationServices.OrderModule.Implements;

public class OrderStatusService : CoreServiceBase, IOrderStatusService
{
    public OrderStatusService(ILogger<OrderStatusService> logger, IHttpContextAccessor httpContext)
        : base(logger, httpContext) { }

    public Task<Result<OrderStatus>> CalculateOrderStatus(int orderId)
    {
        throw new NotImplementedException();
    }
}
