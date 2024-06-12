using Kentico.Xperience.UMT;
using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.DependencyInjection;

using Migration.Tookit.Data.Configuration;
using Migration.Tookit.Data.Models;
using Migration.Tookit.Sitefinity.Abstractions;
using Migration.Tookit.Sitefinity.Configuration;
using Migration.Tookit.Sitefinity.Core.Services;
using Migration.Tookit.Sitefinity.Mappers;
using Migration.Tookit.Sitefinity.Services;
using Migration.Toolkit.Sitefinity.Data;

namespace Migration.Toolkit.Sitefinity;

public static class ServiceCollectionExtensions
{
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
