namespace EFMigrationService.Server.WebAppSettings;

using System.Collections.Generic;

using RaptorUtils.AspNet.Applications;

internal class EFMigrationServiceWebAppDefinition : PluginEnabledWebAppDefinition
{
    protected override ICollection<WebAppPlugin>? GetPlugins()
    {
        return [new EFMigrationServiceSerilogWebAppPlugin()];
    }
}
