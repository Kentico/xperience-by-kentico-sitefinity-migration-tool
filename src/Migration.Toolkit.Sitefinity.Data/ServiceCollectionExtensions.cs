using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Migration.Tookit.Data.Configuration;
using Migration.Tookit.Data.Core.EF;
using Migration.Tookit.Data.Core.Providers;
using Migration.Tookit.Data.Providers;

using Progress.Sitefinity.RestSdk;
using Progress.Sitefinity.RestSdk.Client;

namespace Migration.Toolkit.Sitefinity.Data;
/// <summary>
/// 
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adding the required dependencies and providers to the service collection for pulling data from Sitefinity site
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Sitefinity data configuration used for connections</param>
    /// <returns>Service collection</returns>
    public static IServiceCollection AddSitefinityData(this IServiceCollection services, SitefinityDataConfiguration configuration) =>
        RegisterServices(services, configuration);

    private static IServiceCollection RegisterServices(IServiceCollection services, SitefinityDataConfiguration configuration)
    {
        services.AddSingleton(configuration);

        services.AddHttpClient("sfservice", (servicesProvider, client) =>
        {
            client.BaseAddress = new Uri(configuration.SitefinityRestApiUrl);
            if (!string.IsNullOrEmpty(configuration.SitefinityRestApiKey))
            {
                client.DefaultRequestHeaders.Add("X-SF-APIKEY", configuration.SitefinityRestApiKey);
            }
        }).ConfigurePrimaryHttpMessageHandler(configure => new HttpClientHandler
        {
            AllowAutoRedirect = false,
            UseCookies = false,
        });

        services.AddScoped<IRestClient>((x) =>
        {
            var factory = x.GetRequiredService<IHttpClientFactory>();
            var httpClient = factory.CreateClient("sfservice");

            var restClient = new RestClient(httpClient);
            return restClient;
        });

        services.AddDbContextFactory<SitefinityContext>(options => options.UseSqlServer(configuration.SitefinityConnectionString));
        services.AddTransient<IUserProvider, UserProvider>();
        services.AddTransient<ITypeProvider, TypeProvider>();

        return services;
    }
}
