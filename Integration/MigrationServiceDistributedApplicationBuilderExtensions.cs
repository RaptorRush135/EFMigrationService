namespace EFMigrationService.Integration;

using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

using RaptorUtils.Aspire.Hosting.NodeJs;
using RaptorUtils.Net;

/// <summary>
/// Extension methods for configuring a migration service in a distributed application builder.
/// </summary>
public static class MigrationServiceDistributedApplicationBuilderExtensions
{
    /// <summary>
    /// Adds a migration client to the distributed application builder.
    /// <para>
    /// Configures the migration client with the specified server builder, working directory, name, and optional port.
    /// </para>
    /// <para>
    /// If no port is specified, an available port will be automatically assigned.
    /// </para>
    /// </summary>
    /// <param name="builder">
    /// The distributed application builder to which the migration client will be added.
    /// </param>
    /// <param name="migrationServerBuilder">
    /// The resource builder for the migration server, used to retrieve the server endpoint.
    /// </param>
    /// <param name="location">
    /// The directory in which the migration client is located.
    /// </param>
    /// <param name="name">
    /// The name of the migration client (default is "MigrationClient").
    /// </param>
    /// <param name="port">
    /// The port on which the migration client will run; if null, an available port will be found.
    /// </param>
    /// <returns>
    /// A resource builder for the migration client, configured with the specified parameters.
    /// </returns>
    public static IResourceBuilder<NodeAppResource> AddMigrationClient(
        this IDistributedApplicationBuilder builder,
        IResourceBuilder<ProjectResource> migrationServerBuilder,
        string location,
        string name = "MigrationClient",
        int? port = null)
    {
        int actualPort = port ?? PortFinder.GetAvailablePort();

        EndpointReference serverEndpoint = migrationServerBuilder.GetEndpoint("http");

        return builder
            .AddNpmApp(name, location, "dev")
            .WithEnvironment(
                MigrationServiceEnvironmentVariables.MigrationClientServerUrl,
                () => Path.Join(serverEndpoint.Url, "terminal").Replace('\\', '/'))
            .WithPort(actualPort);
    }
}
