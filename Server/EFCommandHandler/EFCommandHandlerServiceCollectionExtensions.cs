namespace EFMigrationService.Server.EFCommandHandler;

using EFMigrationService.Integration;

using RaptorUtils.Extensions.Configuration;

/// <summary>
/// Provides extension methods for registering the Entity Framework command handler
/// with the dependency injection (DI) container.
/// </summary>
internal static class EFCommandHandlerServiceCollectionExtensions
{
    /// <summary>
    /// Adds the <see cref="EFCommandHandler"/> to the service collection as a singleton.
    /// This method retrieves necessary configuration values for the target and startup projects,
    /// and then registers the command handler with the DI container.
    /// </summary>
    /// <param name="services">The service collection to which the command handler will be added.</param>
    /// <param name="configuration">The application configuration used to retrieve required settings.</param>
    public static void AddEFCommandHandler(this IServiceCollection services, IConfiguration configuration)
    {
        string targetProject = configuration.GetRequired(MigrationServiceEnvironmentVariables.TargetProject);
        string startupProject = configuration.GetRequired(MigrationServiceEnvironmentVariables.StartupProject);

        var commandHandler = new EFCommandHandler(
            MigrationServiceEnvironmentVariables.TargetProject,
            targetProject,
            MigrationServiceEnvironmentVariables.StartupProject,
            startupProject);

        services.AddSingleton(commandHandler);
    }
}
