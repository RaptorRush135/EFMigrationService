namespace EFMigrationService.Server.Hub;

using Microsoft.AspNetCore.SignalR;

using Serilog;
using Serilog.Context;

/// <summary>
/// A hub filter that logs information about client connections and method invocations in a SignalR hub.
/// </summary>
internal class LoggingHubFilter : IHubFilter
{
    /// <summary>
    /// Invokes a hub method and logs the connection ID for the current invocation.
    /// </summary>
    /// <inheritdoc/>
    public async ValueTask<object?> InvokeMethodAsync(
        HubInvocationContext invocationContext,
        Func<HubInvocationContext, ValueTask<object?>> next)
    {
        using (LogContext.PushProperty("ConnectionId", invocationContext.Context.ConnectionId))
        {
            return await next(invocationContext);
        }
    }

    /// <summary>
    /// Invoked when a client connects to the hub. Logs the connection ID.
    /// </summary>
    /// <inheritdoc/>
    public async Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
    {
        using (LogContext.PushProperty("ConnectionId", context.Context.ConnectionId))
        {
            Log.Information("Client connected with ConnectionId: {ConnectionId}", context.Context.ConnectionId);
            await next(context);
        }
    }

    /// <summary>
    /// Invoked when a client disconnects from the hub. Logs the connection ID.
    /// </summary>
    /// <inheritdoc/>
    public async Task OnDisconnectedAsync(HubLifetimeContext context, Exception? exception, Func<HubLifetimeContext, Exception?, Task> next)
    {
        using (LogContext.PushProperty("ConnectionId", context.Context.ConnectionId))
        {
            Log.Information("Client disconnected with ConnectionId: {ConnectionId}", context.Context.ConnectionId);
            await next(context, exception);
        }
    }
}
