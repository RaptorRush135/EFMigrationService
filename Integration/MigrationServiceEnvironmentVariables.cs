namespace EFMigrationService.Integration;

/// <summary>
/// Contains environment variable names and command line arguments related to migration services.
/// </summary>
public static class MigrationServiceEnvironmentVariables
{
    /// <summary>
    /// Environment variable that determines the target project for the migration.
    /// <see href="https://learn.microsoft.com/en-us/ef/core/cli/dotnet#target-project-and-startup-project">
    /// More info
    /// </see>.
    /// </summary>
    public const string TargetProject = "project";

    /// <summary>
    /// Environment variable that determines the startup project for the migration.
    /// <see href="https://learn.microsoft.com/en-us/ef/core/cli/dotnet#target-project-and-startup-project">
    /// More info
    /// </see>.
    /// </summary>
    public const string StartupProject = "startup-project";

    /// <summary>
    /// Environment variable that determines the database persistence mode.
    /// <para>
    /// The data will not be saved between executions if not enabled.
    /// </para>
    /// </summary>
    public const string DatabasePersistenceMode = "DB_PERSISTENCE";

    /// <summary>
    /// Environment variable that determines if EF migration services should be started.
    /// </summary>
    public const string DatabaseMigrationMode = "DB_MIGRATION";

    /// <summary>
    /// The environment variable that determines the server URL for the migration client.
    /// </summary>
    public const string MigrationClientServerUrl = "SERVER_URL";
}
