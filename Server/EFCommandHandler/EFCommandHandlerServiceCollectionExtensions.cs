namespace EFMigrationService.Server.EFCommandHandler;

using EFMigrationService.Integration;

using RaptorUtils.Extensions.Configuration;

internal static class EFCommandHandlerServiceCollectionExtensions
{
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
