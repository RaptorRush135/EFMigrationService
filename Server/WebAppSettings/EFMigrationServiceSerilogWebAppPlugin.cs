namespace EFMigrationService.Server.WebAppSettings;

using RaptorUtils.AspNet.Applications.Plugins.Serilog;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

internal class EFMigrationServiceSerilogWebAppPlugin(
    Func<WebApplicationBuilder, Task<bool>>? isEnabled = null)
    : SerilogWebAppPlugin(isEnabled)
{
    public override ConsoleTheme? ConsoleTheme => AnsiConsoleTheme.Code;

    protected override void ConfigureLogger(IServiceProvider serviceProvider, LoggerConfiguration options)
    {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        options
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: this.ConsoleTheme, applyThemeToRedirectedOutput: true)
            .WriteTo.OpenTelemetry();
    }
}
