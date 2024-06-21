using Kentico.Xperience.UMT;

using Microsoft.Extensions.DependencyInjection;

using Migration.Tookit.Data.Configuration;
using Migration.Tookit.Sitefinity.Core.Services;
using Migration.Tookit.Sitefinity.Services;
using Migration.Toolkit.Sitefinity.Data;

namespace Migration.Toolkit.Sitefinity;

/// <summary>
/// Extension methods for registering Sitefinity Migration Toolkit services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adding Sitefinity Migration Toolkit services to the service collection to use the importer
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="toolkitConfiguration">Sitefinity toolkit configuration used for connections</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddSitefinityMigrationToolkit(this IServiceCollection services, SitefinityToolkitConfiguration toolkitConfiguration) =>
        RegisterServices(services, toolkitConfiguration);

    private static IServiceCollection RegisterServices(IServiceCollection services, SitefinityToolkitConfiguration toolkitConfiguration)
    {
        services.AddUniversalMigrationToolkit();
        services.AddSitefinityData(toolkitConfiguration);

        services.AddTransient<IUserImportService, UserImportService>();
        services.AddTransient<ISitefinityImportService, SitefinityImportService>();

        return services;
    }
}
