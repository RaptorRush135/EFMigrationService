namespace EFMigrationService.Integration;

using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

/// <summary>
/// Provides extension methods for configuring migration-related resources using a resource builder.
/// </summary>
public static class MigrationServiceResourceBuilderExtensions
{
    /// <summary>
    /// Configures the resource builder with a single project as both the migration target
    /// and startup project using the specified project path.
    /// <para>
    /// This sets both the <see cref="MigrationServiceEnvironmentVariables.TargetProject"/> and
    /// <see cref="MigrationServiceEnvironmentVariables.StartupProject"/> variables to the given project path.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of resource that includes environment configuration.</typeparam>
    /// <param name="builder">The resource builder to configure.</param>
    /// <param name="projectPath">The path of the project to use as both the migration target and startup project.</param>
    /// <returns>The modified resource builder.</returns>
    public static IResourceBuilder<T> WithSingleMigrationProject<T>(this IResourceBuilder<T> builder, string projectPath)
        where T : IResourceWithEnvironment
    {
        return builder
            .WithMigrationTargetProject(projectPath)
            .WithMigrationStartupProject(projectPath);
    }

    /// <summary>
    /// Configures the resource builder with the specified project path as the migration target project.
    /// <para>
    /// This sets the <see cref="MigrationServiceEnvironmentVariables.TargetProject"/> variable
    /// to the given project path.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of resource that includes environment configuration.</typeparam>
    /// <param name="builder">The resource builder to configure.</param>
    /// <param name="projectPath">The path of the project to use as the migration target.</param>
    /// <returns>The modified resource builder.</returns>
    public static IResourceBuilder<T> WithMigrationTargetProject<T>(this IResourceBuilder<T> builder, string projectPath)
        where T : IResourceWithEnvironment
    {
        return builder.WithEnvironment(MigrationServiceEnvironmentVariables.TargetProject, projectPath);
    }

    /// <summary>
    /// Configures the resource builder with the specified project path as the migration startup project.
    /// <para>
    /// This sets the <see cref="MigrationServiceEnvironmentVariables.StartupProject"/> variable
    /// to the given project path.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of resource that includes environment configuration.</typeparam>
    /// <param name="builder">The resource builder to configure.</param>
    /// <param name="projectPath">The path of the project to use as the migration startup.</param>
    /// <returns>The modified resource builder.</returns>
    public static IResourceBuilder<T> WithMigrationStartupProject<T>(this IResourceBuilder<T> builder, string projectPath)
        where T : IResourceWithEnvironment
    {
        return builder.WithEnvironment(MigrationServiceEnvironmentVariables.StartupProject, projectPath);
    }
}
