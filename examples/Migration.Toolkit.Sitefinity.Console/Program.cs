﻿#pragma warning disable S1135 // this is sample, todos are here for end user
// See https://aka.ms/new-console-template for more information

using System.Text.Json;

using CMS.Core;
using CMS.DataEngine;

//using CMS.DataEngine;

//using Kentico.Xperience.UMT;
using Kentico.Xperience.UMT.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Migration.Tookit.Data.Configuration;
using Migration.Tookit.Sitefinity.Core.Services;






//using Microsoft.Extensions.Logging;

using Migration.Toolkit.Sitefinity;

var root = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false)
    .AddJsonFile("appsettings.local.json", true)
    .Build();

Service.Use<IConfiguration>(root);
CMS.Base.SystemContext.WebApplicationPhysicalPath = root.GetValue<string>("WebApplicationPhysicalPath");

CMSApplication.Init();

var services = new ServiceCollection();
services.AddLogging(b => b.AddDebug().AddSimpleConsole(options => options.SingleLine = true).AddConfiguration(root.GetSection("Logging")));
services.AddSitefinityMigrationToolkit(new SitefinityToolkitConfiguration
{
    SitefinityConnectionString = root.GetValue<string>("ConnectionStrings:SitefinityConnectionString") ?? "",
    SitefinityRestApiUrl = root.GetValue<string>("SitefinityRestApiUrl") ?? "",
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
var observer = importService.StartImportUsers(importObserver);

// wait until import finishes
await observer.ImportCompletedTask;
//importService.StartImport(users);

//// sample data
//List<IUmtModel> sourceData = null!;

//sourceData = importService.FromJsonString(SampleJson.FULL_SAMPLE)?.ToList() ?? [];

//bool variantWithObserver = false;
//if (variantWithObserver)
//{
//    // simplified usage for streamlined import

//    // create observer to track import state
//    var importObserver = new ImportStateObserver();

//    // listen to validation errors
//    //importObserver.ValidationError += (model, uniqueId, errors) =>
//    //{
//    //    Console.WriteLine($"Validation error in model '{model.PrintMe()}': {JsonSerializer.Serialize(errors)}");
//    //};

//    //// listen to successfully adapted and persisted objects
//    //importObserver.ImportedInfo += (model, info) =>
//    //{
//    //    Console.WriteLine($"{model.PrintMe()} imported");
//    //};

//    //// listen for exception occurence
//    //importObserver.Exception += (model, uniqueId, exception) =>
//    //{
//    //    Console.WriteLine($"Error in model {model.PrintMe()}: '{uniqueId}': {exception}");
//    //};

//    // initiate import
//    var observer = importService.StartImport(sourceData, importObserver);

//    // wait until import finishes
//    await observer.ImportCompletedTask;
//}
//else
//{
//    // sample with more control over process
//    var importer = serviceProvider.GetRequiredService<IImporter>();
//    foreach (var umtModel in sourceData)
//    {
//        var result = await importer.ImportAsync(umtModel);
//        switch (result)
//        {
//            // OK
//            case { Success: true }:
//            {
//                Console.WriteLine($"{umtModel.PrintMe()} imported");
//                break;
//            }
//            // some exception was thrown when importing
//            case { Success: false, Exception: { } exception }:
//            {
//                Console.WriteLine($"Error in model {umtModel.PrintMe()}: {exception}");
//                break;
//            }
//            // validation error were found on input model
//            case { Success: false, ModelValidationResults: { } validationResults }:
//            {
//                Console.WriteLine($"Validation error in model '{umtModel.PrintMe()}': {JsonSerializer.Serialize(validationResults)}");
//                break;
//            }
//            default:
//            {
//                Console.WriteLine($"UNEXPECTED CASE occured on model: {umtModel.PrintMe()}");
//                break;
//            }
//        }
//    }
//}

Console.WriteLine("Finished!");

#pragma warning restore S1135
