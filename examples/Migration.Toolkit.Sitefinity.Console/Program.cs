#pragma warning disable S1135 // this is sample, todos are here for end user
// See https://aka.ms/new-console-template for more information

using System.Text.Json;

using CMS.Core;
using CMS.DataEngine;

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

CMSApplication.Init();

var services = new ServiceCollection();
services.AddLogging(b => b.AddDebug().AddSimpleConsole(options => options.SingleLine = true).AddConfiguration(root.GetSection("Logging")));
services.AddSitefinityMigrationToolkit(new SitefinityDataConfiguration
{
    SitefinityConnectionString = root.GetValue<string>("ConnectionStrings:SitefinityConnectionString") ?? "",
    SitefinitySiteUrl = root.GetValue<string>("Sitefinity:Url") ?? "",
    SitefinityRestApiUrl = (root.GetValue<string>("Sitefinity:Url") ?? "") + root.GetValue<string>("Sitefinity:WebServicePath") ?? "",
    SitefinityModuleDeploymentFolderPath = root.GetValue<string>("Sitefinity:ModuleDeploymentFolderPath") ?? "",
}, new SitefinityImportConfiguration
{
    SitefinityCodeNamePrefix = root.GetValue<string>("Sitefinity:CodeNamePrefix") ?? "",
    PageContentTypes = root.GetSection("Sitefinity:PageContentTypes").Get<List<PageContentType>>()
});

var serviceProvider = services.BuildServiceProvider();
var importService = serviceProvider.GetRequiredService<ISitefinityImportService>();

var importObserver = new ImportStateObserver();

// listen to validation errors
importObserver.ValidationError += (model, uniqueId, errors) => Console.WriteLine($"Validation error in model '{model.PrintMe()}': {JsonSerializer.Serialize(errors)}");

// listen to successfully adapted and persisted objects
importObserver.ImportedInfo += (model, info) => Console.WriteLine($"{model.PrintMe()} imported");

// listen for exception occurence
importObserver.Exception += (model, uniqueId, exception) => Console.WriteLine($"Error in model {model.PrintMe()}: '{uniqueId}': {exception}");

// initiate import
var observer = importService.StartImportMedia(importObserver);

// wait until import finishes
await observer.ImportCompletedTask;

Console.WriteLine("Finished!");

#pragma warning restore S1135
