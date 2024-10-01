namespace EFMigrationService.Server.WebAppSettings;

using System.Collections.Generic;

using RaptorUtils.AspNet.Applications;

/// <summary>
/// Defines the web application for the EF migration service, integrating necessary plugins for enhanced functionality.
/// </summary>
internal class EFMigrationServiceWebAppDefinition : PluginEnabledWebAppDefinition
{
    /// <summary>
    /// Retrieves the collection of plugins to be used by the web application.
    /// This implementation includes the <see cref="EFMigrationServiceSerilogWebAppPlugin"/>
    /// for enhanced logging capabilities.
    /// </summary>
    /// <returns>A collection of web application plugins.</returns>
    protected override ICollection<WebAppPlugin>? GetPlugins()
    {
        return [new EFMigrationServiceSerilogWebAppPlugin()];
    }
}
