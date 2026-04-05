using CR.ConstantBase.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace CR.InfrastructureBase.Logging;

public static class LoggerExtensions
{
    public static IDisposable LogXRequestId(this ILogger logger, string? requestId)
    {
        var requestIds = new StringValues([requestId]);
        return Serilog.Context.LogContext.PushProperty(LogPropertyNames.XRequestId, requestIds);
    }
}
