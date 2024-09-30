namespace EFMigrationService.Server;

using System.Threading.Tasks;

using EFMigrationService.Server.EFCommandHandler;
using EFMigrationService.Server.Hub;
using EFMigrationService.Server.WebAppSettings;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;

internal class MigrationService : EFMigrationServiceWebAppDefinition
{
    protected override async Task ConfigureServices(WebApplicationBuilder builder)
    {
        await base.ConfigureServices(builder);

        builder.Services.AddSignalR(options => options.AddFilter<LoggingHubFilter>());

        builder.Services.AddEFCommandHandler(builder.Configuration);
    }

    protected override async Task Configure(WebApplication app)
    {
        await base.Configure(app);

        app.MapHub<TerminalHub>("/terminal");
    }
}
