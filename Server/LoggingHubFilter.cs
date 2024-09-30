namespace MrHotel.MigrationService;

using Microsoft.AspNetCore.SignalR;

using Serilog;
using Serilog.Context;

public class LoggingHubFilter : IHubFilter
{
    public async ValueTask<object?> InvokeMethodAsync(HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object?>> next)
    {
        using (LogContext.PushProperty("ConnectionId", invocationContext.Context.ConnectionId))
        {
            return await next(invocationContext);
        }
    }

    public async Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
    {
        using (LogContext.PushProperty("ConnectionId", context.Context.ConnectionId))
        {
            Log.Information("Client connected with ConnectionId: {ConnectionId}", context.Context.ConnectionId);
            await next(context);
        }
    }

    public async Task OnDisconnectedAsync(HubLifetimeContext context, Exception? exception, Func<HubLifetimeContext, Exception?, Task> next)
    {
        using (LogContext.PushProperty("ConnectionId", context.Context.ConnectionId))
        {
            Log.Information("Client disconnected with ConnectionId: {ConnectionId}", context.Context.ConnectionId);
            await next(context, exception);
        }
    }
}
