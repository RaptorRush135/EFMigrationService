namespace EFMigrationService.Integration;

using Microsoft.Extensions.Configuration;

using RaptorUtils.Extensions.Configuration;

/// <summary>
/// Provides extension methods for checking if certain migration-related modes
/// are enabled in the application configuration.
/// </summary>
public static class MigrationServiceConfigurationExtensions
{
    /// <summary>
    /// Determines whether the database persistence is enabled based on the provided configuration settings.
    /// <para>
    /// Database persistence is enabled if the
    /// <see cref="MigrationServiceEnvironmentVariables.DatabasePersistenceMode"/> or
    /// <see cref="MigrationServiceEnvironmentVariables.DatabaseMigrationMode"/>
    /// environment variable is set.
    /// </para>
    /// </summary>
    /// <param name="configuration">The application configuration used to check environment variables.</param>
    /// <returns>
    /// <see langword="true"/> if database persistence is enabled; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsDatabasePersistenceEnabled(this IConfiguration configuration)
    {
        return configuration.IsEnabled(MigrationServiceEnvironmentVariables.DatabasePersistenceMode)
            || configuration.IsDatabaseMigrationMode();
    }

    /// <summary>
    /// Determines whether the application is in database migration mode based on the provided configuration settings.
    /// <para>
    /// The migration mode is active if the <see cref="MigrationServiceEnvironmentVariables.DatabaseMigrationMode"/>
    /// variable is set.
    /// </para>
    /// </summary>
    /// <param name="configuration">The application configuration used to check environment variables.</param>
    /// <returns>
    /// <see langword="true"/> if the application is in database migration mode; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool IsDatabaseMigrationMode(this IConfiguration configuration)
    {
        return configuration.IsEnabled(MigrationServiceEnvironmentVariables.DatabaseMigrationMode);
    }
}
