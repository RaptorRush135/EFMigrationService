namespace EFMigrationService.Integration;

/// <summary>
/// Contains environment variable names and command line arguments related to migration services.
/// </summary>
public static class MigrationServiceEnvironmentVariables
{
    /// <summary>
    /// Command line argument for specifying the target project when running migrations.
    /// </summary>
    public const string TargetProject = "--project";

    /// <summary>
    /// Command line argument for specifying the startup project when running migrations.
    /// </summary>
    public const string StartupProject = "--startup-project";

    /// <summary>
    /// Environment variable that enables database persistence mode.
    /// <para>
    /// The data will not be saved between executions if not enabled.
    /// </para>
    /// </summary>
    public const string DatabasePersistenceMode = "DB_PERSISTENCE";

    /// <summary>
    /// Environment variable that determines if EF migration services should be started.
    /// </summary>
    public const string DatabaseMigrationMode = "DB_MIGRATION";
}
