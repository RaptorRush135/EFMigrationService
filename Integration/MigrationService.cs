namespace EFMigrationService.Integration;

using System.Text;
using System.Text.Json;

using CliWrap;

using Microsoft.Extensions.Logging;

/// <summary>
/// Provides data related to the migration service and facilitates the installation of Entity Framework (EF) tools.
/// </summary>
public sealed class MigrationService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MigrationService"/> class.
    /// </summary>
    /// <param name="logger">The logger instance used for logging messages.</param>
    /// <param name="location">The file system location where the migration service is located.</param>
    /// <param name="efToolVersion">The version of the Entity Framework tool to install.</param>
    public MigrationService(
        ILogger<MigrationService> logger,
        string location,
        string efToolVersion)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(location);
        ArgumentNullException.ThrowIfNull(efToolVersion);

        this.Logger = logger;
        this.FullPath = Path.GetFullPath(location);
        this.ServerPath = Path.Combine(this.FullPath, "Server");
        this.ClientPath = Path.Combine(this.FullPath, "Client");
        this.EFToolVersion = efToolVersion;
    }

    /// <summary>
    /// Gets the full path to the root migration service directory.
    /// </summary>
    public string FullPath { get; }

    /// <summary>
    /// Gets the path to the server-side migration service.
    /// </summary>
    public string ServerPath { get; }

    /// <summary>
    /// Gets the path to the client-side migration service.
    /// </summary>
    public string ClientPath { get; }

    /// <summary>
    /// Gets the version of the Entity Framework tool to install.
    /// </summary>
    public string EFToolVersion { get; }

    private ILogger<MigrationService> Logger { get; }

    /// <summary>
    /// Ensures the EF tool is installed.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Initialize()
    {
        if (await this.CheckIfEFToolInstalled())
        {
            this.Logger.LogInformation("EF tool already installed");
            return;
        }

        this.Logger.LogInformation("Installing EF tool ({EFVersion})...", this.EFToolVersion);

        await this.ExecuteDotnetCommand("new tool-manifest --force");

        await this.ExecuteDotnetCommand($"tool install dotnet-ef --version {this.EFToolVersion}");
    }

    private async Task<bool> CheckIfEFToolInstalled()
    {
        try
        {
            string manifestPath = Path.Combine(this.ServerPath, ".config", "dotnet-tools.json");
            if (!File.Exists(manifestPath))
            {
                return false;
            }

            string manifestText = await File.ReadAllTextAsync(manifestPath);

            using var doc = JsonDocument.Parse(manifestText);

            string? toolVersion = doc.RootElement
                .GetProperty("tools")
                .GetProperty("dotnet-ef")
                .GetProperty("version")
                .GetString();

            return toolVersion == this.EFToolVersion;
        }
        catch (Exception e)
        {
            this.Logger.LogDebug(e, "Error checking tool version");
            return false;
        }
    }

    private async Task<CommandResult> ExecuteDotnetCommand(
        string arguments)
    {
        var stdOutBuffer = new StringBuilder();
        var stdErrBuffer = new StringBuilder();
        string stdOut;
        string stdErr;

        try
        {
            await Cli.Wrap("dotnet")
                .WithArguments(arguments)
                .WithWorkingDirectory(this.ServerPath)
                .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
                .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdErrBuffer))
                .ExecuteAsync();
        }
        finally
        {
            LogResult(stdOutBuffer, LogLevel.Debug, out stdOut);
            LogResult(stdErrBuffer, LogLevel.Error, out stdErr);

            void LogResult(StringBuilder buffer, LogLevel logLevel, out string result)
            {
                result = buffer.ToString().Trim();
                if (result.Length > 0)
                {
                    this.Logger.Log(logLevel, "{CommandResult}", result);
                }
            }
        }

        return new CommandResult(stdOut, stdErr);
    }
}
