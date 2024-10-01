namespace EFMigrationService.Integration;

using Microsoft.Extensions.Configuration;

using RaptorUtils.Extensions.Configuration;

public static class MigrationServiceConfigurationExtensions
{
    public static bool IsDatabasePersistenceEnabled(this IConfiguration configuration)
    {
        return configuration.IsEnabled(MigrationServiceEnvironmentVariables.DatabasePersistenceMode)
            || configuration.IsDatabaseMigrationMode();
    }

    public static bool IsDatabaseMigrationMode(this IConfiguration configuration)
    {
        return configuration.IsEnabled(MigrationServiceEnvironmentVariables.DatabaseMigrationMode);
    }
}
