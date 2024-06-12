using Kentico.Xperience.UMT;
using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.DependencyInjection;

using Migration.Toolkit.Data.Configuration;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Adapters;
using Migration.Toolkit.Sitefinity.Configuration;
using Migration.Toolkit.Sitefinity.Core.Services;
using Migration.Toolkit.Sitefinity.Data;
using Migration.Toolkit.Sitefinity.Services;

namespace Migration.Toolkit.Sitefinity;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adding Sitefinity Migration Toolkit services to the service collection to use the importer
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="toolkitConfiguration">Sitefinity toolkit configuration used for connections</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddSitefinityMigrationToolkit(this IServiceCollection services, SitefinityDataConfiguration dataConfiguration, SitefinityImportConfiguration importConfiguration) =>
        RegisterServices(services, dataConfiguration, importConfiguration);

    private static IServiceCollection RegisterServices(IServiceCollection services, SitefinityDataConfiguration dataConfiguration, SitefinityImportConfiguration importConfiguration)
    {
        services.AddUniversalMigrationToolkit();
        services.AddSitefinityData(dataConfiguration);
        services.AddSingleton(importConfiguration);

        // Services
        services.AddTransient<IUserImportService, UserImportService>();
        services.AddTransient<IDataClassImportService, DataClassImportService>();
        services.AddTransient<ISitefinityImportService, SitefinityImportService>();

        // Mappers
        services.AddTransient<IUmtAdapter<User, UserInfoModel>, UserInfoModelAdapter>();
        services.AddTransient<IUmtAdapter<SitefinityType, DataClassModel>, DataClassModelAdapter>();

        return services;
    }
}
