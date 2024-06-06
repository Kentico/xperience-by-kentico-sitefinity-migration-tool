using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Migration.Tookit.Data.Configuration;
using Migration.Tookit.Data.Core.EF;

using Progress.Sitefinity.RestSdk;
using Progress.Sitefinity.RestSdk.Client;

//using Progress.Sitefinity.AspNetCore;

namespace Migration.Toolkit.Sitefinity.Data
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddSitefinityData(this IServiceCollection services, SitefinityToolkitConfiguration configuration) =>
            RegisterServices(services, configuration);

        private static IServiceCollection RegisterServices(IServiceCollection services, SitefinityToolkitConfiguration configuration)
        {
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

            return services;
        }
    }
}
