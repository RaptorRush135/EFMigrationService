namespace EFMigrationService.Server;

using System.Threading.Tasks;

using EFMigrationService.Server.EFCommandHandler;
using EFMigrationService.Server.Hub;
using EFMigrationService.Server.WebAppSettings;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;

/// <summary>
/// Defines the migration service used for configuring and running migrations.
/// </summary>
internal class MigrationService : EFMigrationServiceWebAppDefinition
{
    /// <summary>
    /// Configures services for the application, including SignalR and an Entity Framework command handler.
    /// </summary>
    /// <inheritdoc/>
    protected override async Task ConfigureServices(WebApplicationBuilder builder)
    {
        await base.ConfigureServices(builder);

        builder.Services.AddSignalR(options => options.AddFilter<LoggingHubFilter>());

        builder.Services.AddEFCommandHandler(builder.Configuration);
    }

    /// <summary>
    /// Configures the application's request pipeline, including mapping the TerminalHub SignalR hub.
    /// </summary>
    /// <inheritdoc/>
    protected override async Task Configure(WebApplication app)
    {
        await base.Configure(app);

        app.MapHub<TerminalHub>("/terminal");
    }
}
