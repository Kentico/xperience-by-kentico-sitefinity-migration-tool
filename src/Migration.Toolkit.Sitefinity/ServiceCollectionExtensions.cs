using Kentico.Xperience.UMT;

using Microsoft.Extensions.DependencyInjection;

using Migration.Tookit.Data.Configuration;
using Migration.Tookit.Sitefinity.Core.Services;
using Migration.Tookit.Sitefinity.Services;
using Migration.Toolkit.Sitefinity.Data;

namespace Migration.Toolkit.Sitefinity;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSitefinityMigrationToolkit(this IServiceCollection services, SitefinityToolkitConfiguration toolkitConfiguration) =>
        RegisterServices(services, toolkitConfiguration);

    private static IServiceCollection RegisterServices(IServiceCollection services, SitefinityToolkitConfiguration toolkitConfiguration)
    {
        services.AddUniversalMigrationToolkit();
        services.AddSitefinityData(toolkitConfiguration);

        services.AddTransient<ISitefinityImportService, SitefinityImportService>();

        return services;
    }
}
