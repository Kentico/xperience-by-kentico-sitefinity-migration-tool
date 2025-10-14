using System.Text.Json;

using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;

using Kentico.Xperience.UMT.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Configuration;

using Migration.Toolkit.Sitefinity;
using Migration.Toolkit.Sitefinity.Configuration;
using Migration.Toolkit.Sitefinity.Core.Services;

var root = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false)
    .AddJsonFile("appsettings.local.json", true)
    .Build();

Service.Use<IConfiguration>(root);
CMS.Base.SystemContext.WebApplicationPhysicalPath = root.GetValue<string>("WebApplicationPhysicalPath");

// Set the current directory to the web application path so assets are written to the correct location
Directory.SetCurrentDirectory(root.GetValue<string>("WebApplicationPhysicalPath") ?? throw new InvalidOperationException("WebApplicationPhysicalPath must be set"));

CMSApplication.Init();

var services = new ServiceCollection();
services.AddLogging(b => b.AddDebug().AddSimpleConsole(options => options.SingleLine = true).AddConfiguration(root.GetSection("Logging")));
services.AddSitefinityMigrationToolkit(new SitefinityDataConfiguration
{
    SitefinityConnectionString = root.GetValue<string>("ConnectionStrings:SitefinityConnectionString") ?? "",
    SitefinitySiteDomain = root.GetValue<string>("Sitefinity:Domain") ?? "",
    SitefinityRestApiUrl = "https://" + root.GetValue<string>("Sitefinity:Domain") + root.GetValue<string>("Sitefinity:WebServicePath"),
    SitefinityModuleDeploymentFolderPath = root.GetValue<string>("Sitefinity:ModuleDeploymentFolderPath") ?? ""
}, new SitefinityImportConfiguration
{
    SitefinityCodeNamePrefix = root.GetValue<string>("Sitefinity:CodeNamePrefix") ?? "",
    PageContentTypes = root.GetSection("Sitefinity:PageContentTypes").Get<List<PageContentType>>(),
    KenticoDefaultWorkspaceName = root.GetValue<string>("Sitefinity:KenticoDefaultWorkspaceName") ?? "KenticoDefault",
    KenticoAdministratorUserName = root.GetValue<string>("Sitefinity:KenticoAdministratorUserName") ?? "administrator",
});

var serviceProvider = services.BuildServiceProvider();
var importService = serviceProvider.GetRequiredService<ISitefinityImportService>();
var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

var importObserver = new ImportStateObserver();

// listen to validation errors
importObserver.ValidationError += (model, uniqueId, errors) =>
    logger.LogError("Validation error in model '{Model}': {Errors}",
                    model.PrintMe(),
                    JsonSerializer.Serialize(errors));

// listen to successfully adapted and persisted objects
importObserver.ImportedInfo += (model, info) =>
    logger.LogInformation("{Model} imported",
                          model.PrintMe());

// listen for exception occurence
importObserver.Exception += (model, uniqueId, exception) =>
    logger.LogError(exception,
                    "Error in model {Model}: '{UniqueId}'",
                    model.PrintMe(),
                    uniqueId);

// initiate import
var observer = importService.StartImportContent(importObserver);

// wait until import finishes
await observer.ImportCompletedTask;

CacheHelper.ClearCache();
logger.LogInformation("Cache Cleared!");
logger.LogInformation("Finished!");

