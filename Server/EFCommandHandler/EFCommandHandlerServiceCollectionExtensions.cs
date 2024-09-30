namespace EFMigrationService.Server.EFCommandHandler;

using RaptorUtils.Extensions.Configuration;

public static class EFCommandHandlerServiceCollectionExtensions
{
    private const string TargetProjectKey = "--project";

    private const string StartupProjectKey = "--startup-project";

    public static void AddEFCommandHandler(this IServiceCollection services, IConfiguration configuration)
    {
        string project = configuration.GetRequired(TargetProjectKey);
        string startupProject = configuration.GetRequired(StartupProjectKey);

        var commandHandler = new EFCommandHandler(TargetProjectKey, project, StartupProjectKey, startupProject);
        services.AddSingleton(commandHandler);
    }
}
