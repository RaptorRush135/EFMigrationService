namespace EFMigrationService.Integration;

using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

public static class MigrationServiceResourceBuilderExtensions
{
    public static IResourceBuilder<T> WithSingleMigrationProject<T>(this IResourceBuilder<T> builder, string projectPath)
        where T : IResourceWithEnvironment
    {
        return builder
            .WithMigrationTargetProject(projectPath)
            .WithMigrationStartupProject(projectPath);
    }

    public static IResourceBuilder<T> WithMigrationTargetProject<T>(this IResourceBuilder<T> builder, string projectPath)
        where T : IResourceWithEnvironment
    {
        return builder.WithEnvironment(MigrationServiceEnvironmentVariables.TargetProject, projectPath);
    }

    public static IResourceBuilder<T> WithMigrationStartupProject<T>(this IResourceBuilder<T> builder, string projectPath)
        where T : IResourceWithEnvironment
    {
        return builder.WithEnvironment(MigrationServiceEnvironmentVariables.StartupProject, projectPath);
    }
}
