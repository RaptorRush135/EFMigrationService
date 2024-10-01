namespace EFMigrationService.Server.WebAppSettings;

using RaptorUtils.AspNet.Applications.Plugins.Serilog;

using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

/// <summary>
/// A plugin for integrating Serilog logging capabilities into the EF migration service within a web application.
/// </summary>
/// <param name="isEnabled">An optional function that determines if the plugin is enabled.</param>
internal sealed class EFMigrationServiceSerilogWebAppPlugin(
    Func<WebApplicationBuilder, Task<bool>>? isEnabled = null)
    : SerilogWebAppPlugin(isEnabled)
{
    /// <summary>
    /// Gets the console theme used for logging output.
    /// This implementation uses the <see cref="AnsiConsoleTheme.Code"/> theme.
    /// </summary>
    public override ConsoleTheme? ConsoleTheme => AnsiConsoleTheme.Code;

    /// <summary>
    /// Configures the logger settings for the web application using the provided service provider.
    /// <para>
    /// This method sets up logging options, including reading from the configuration, enriching logs,
    /// and writing logs to the console and OpenTelemetry.
    /// </para>
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
    /// <param name="options">The logger configuration options to be set up.</param>
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
