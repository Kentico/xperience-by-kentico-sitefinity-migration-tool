using System.Text.Json;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Configuration;
using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Data;

namespace Migration.Toolkit.Data.Providers;

internal class TypeProvider(SitefinityDataConfiguration configuration, ILogger<TypeProvider> logger) : ITypeProvider
{
    private readonly string[] excludedFileNames = ["version.sf", "configs.sf", "widgetTemplates.sf"];
    private readonly string[] sitefinityTypeDirectories = ["Blogs", "Events", "Lists", "News"];
    private readonly string staticSitefinityTypesDirectory = Path.Combine(AppContext.BaseDirectory, "StaticSitefinityTypes");

    public IEnumerable<SitefinityType> GetAllTypes()
    {
        var sitefinityTypes = new List<SitefinityType>();

        sitefinityTypes.AddRange(GetDynamicModuleTypes());
        sitefinityTypes.AddRange(GetSitefinityTypes());
        sitefinityTypes.AddRange(GetMediaContentTypes());

        return sitefinityTypes;
    }

    /// <summary>
    /// Get the default media content type definitions for Image, Download, Video.
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// The GUIDs for the media content types and their fields are hardcoded to support
    /// rerunning the migration on a target XbyK instance without corrupting
    /// type definitions between runs.
    /// </remarks>
    public IEnumerable<SitefinityType> GetMediaContentTypes()
    {
        var mediaTypes = new List<SitefinityType>();

        // Create Image content type
        var imageFields = CreateImageFields();
        var imageType = new StaticSitefinityType
        {
            Id = Guid.Parse("4C86237A-A336-4668-A681-DD47D00581F0"),
            DisplayName = "Image",
            ClassName = SitefinityMigrationConstants.MigratedImageDefaultTypeName,
            Namespace = SitefinityMigrationConstants.MigratedFileTypeDefaultNamespace,
            Fields = imageFields,
            LastModified = DateTime.Now
        };
        mediaTypes.Add(imageType);

        // Create Download content type
        var downloadFields = CreateDownloadFields();
        var downloadType = new StaticSitefinityType
        {
            Id = Guid.Parse("CC922711-DC5E-412A-947C-3457D495E528"),
            DisplayName = "Download",
            ClassName = SitefinityMigrationConstants.MigratedDownloadDefaultTypeName,
            Namespace = SitefinityMigrationConstants.MigratedFileTypeDefaultNamespace,
            Fields = downloadFields,
            LastModified = DateTime.Now
        };
        mediaTypes.Add(downloadType);

        // Create Video content type
        var videoFields = CreateVideoFields();
        var videoType = new StaticSitefinityType
        {
            Id = Guid.Parse("65B6339B-F18D-4D2E-AF69-B9C8F7820C32"),
            DisplayName = "Video",
            ClassName = SitefinityMigrationConstants.MigratedVideoDefaultTypeName,
            Namespace = SitefinityMigrationConstants.MigratedFileTypeDefaultNamespace,
            Fields = videoFields,
            LastModified = DateTime.Now
        };
        mediaTypes.Add(videoType);

        return mediaTypes;
    }

    private static List<Field> CreateImageFields() =>
    [
        new()
        {
            Id = Guid.Parse("E0855E48-8962-40A3-ADEA-00CD84850C85"),
            Name = "ImageTitle",
            Title = "Title",
            ColumnName = "ImageTitle",
            WidgetTypeName = "Kentico.Administration.TextInput",
            IsRequired = false
        },
        new()
        {
            Id = Guid.Parse("F0FAC065-E9CE-403E-A4A3-3915F8E0F7E6"),
            Name = "ImageDescription",
            Title = "Description",
            ColumnName = "ImageDescription",
            WidgetTypeName = "Kentico.Administration.TextInput",
            IsRequired = false
        },
        new()
        {
            Id = Guid.Parse("30F44E4C-BCDC-46D4-B5FA-00455F7456AC"),
            Name = "ImageAsset",
            Title = "Asset",
            ColumnName = "ImageAsset",
            WidgetTypeName = "Kentico.Administration.ContentItemAssetUploader",
            IsRequired = false
        },
        new()
        {
            Id = Guid.Parse("15E39897-1A49-440F-9EF2-D8F6151B3569"),
            Name = "ImageAssetLegacyUrl",
            Title = "Asset Legacy Url",
            ColumnName = "ImageAssetLegacyUrl",
            WidgetTypeName = "Kentico.Administration.TextInput",
            IsRequired = false
        }
    ];

    private static List<Field> CreateDownloadFields() =>
    [
        new()
        {
            Id = Guid.Parse("BE4847B8-4E0C-4341-9CC2-E4CF86DF8681"),
            Name = "DownloadTitle",
            Title = "Title",
            ColumnName = "DownloadTitle",
            WidgetTypeName = "Kentico.Administration.TextInput",
            IsRequired = false
        },
        new()
        {
            Id = Guid.Parse("0D4C579E-87C2-4631-A6AE-FA3792C36C52"),
            Name = "DownloadDescription",
            Title = "Description",
            ColumnName = "DownloadDescription",
            WidgetTypeName = "Kentico.Administration.TextInput",
            IsRequired = false
        },
        new()
        {
            Id = Guid.Parse("296C7E42-8B3C-42FB-8278-9AE57D7759D5"),
            Name = "DownloadAsset",
            Title = "Asset",
            ColumnName = "DownloadAsset",
            WidgetTypeName = "Kentico.Administration.ContentItemAssetUploader",
            IsRequired = false
        },
        new()
        {
            Id = Guid.Parse("30B652D1-D071-4FED-955E-BC7A5E0C260A"),
            Name = "DownloadAssetUrl",
            Title = "Asset Legacy Url",
            ColumnName = "DownloadAssetLegacyUrl",
            WidgetTypeName = "Kentico.Administration.TextInput",
            IsRequired = false
        }
    ];

    private static List<Field> CreateVideoFields() =>
    [
        new()
        {
            Id = Guid.Parse("5155224C-CB91-44D7-AD92-5C60C04C144C"),
            Name = "VideoTitle",
            Title = "Title",
            ColumnName = "VideoTitle",
            WidgetTypeName = "Kentico.Administration.TextInput",
            IsRequired = false
        },
        new()
        {
            Id = Guid.Parse("5BC6605F-ACC1-4DC4-89DF-5AE9FEC52435"),
            Name = "VideoDescription",
            Title = "Description",
            ColumnName = "VideoDescription",
            WidgetTypeName = "Kentico.Administration.TextInput",
            IsRequired = false
        },
        new()
        {
            Id = Guid.Parse("FE21A4FA-9C3E-440C-A39D-9F7471B21F56"),
            Name = "VideoAsset",
            Title = "Asset",
            ColumnName = "VideoAsset",
            WidgetTypeName = "Kentico.Administration.ContentItemAssetUploader",
            IsRequired = false
        },
        new()
        {
            Id = Guid.Parse("BF390CBC-437B-4988-AD28-F83EFE25A125"),
            Name = "VideoAssetUrl",
            Title = "Asset Legacy Url",
            ColumnName = "VideoAssetLegacyUrl",
            WidgetTypeName = "Kentico.Administration.TextInput",
            IsRequired = false
        }
    ];

    public IEnumerable<SitefinityType> GetDynamicModuleTypes()
    {
        string dynamicModulesPath = configuration.SitefinityModuleDeploymentFolderPath + "\\Dynamic modules";

        if (!Path.IsPathRooted(configuration.SitefinityModuleDeploymentFolderPath))
        {
            dynamicModulesPath = Environment.CurrentDirectory + configuration.SitefinityModuleDeploymentFolderPath + "\\Dynamic modules";
        }

        if (string.IsNullOrEmpty(dynamicModulesPath) || !Directory.Exists(dynamicModulesPath))
        {
            logger.LogError("Sitefinity module deployment folder does not exist. {DynamicModulesPath}", dynamicModulesPath);
            return [];
        }

        var dynamicTypes = new List<DynamicModuleType>();

        foreach (string path in Directory.EnumerateFiles(dynamicModulesPath, "*.sf", SearchOption.AllDirectories)
                                            .Where(path => !Array.Exists(excludedFileNames, e => e.Equals(Path.GetFileName(path)))))
        {
            string fileContents = File.ReadAllText(path);

            if (string.IsNullOrEmpty(fileContents))
            {
                logger.LogWarning("File {Path} is empty.", path);
                continue;
            }

            var module = JsonSerializer.Deserialize<Module>(fileContents);

            if (module == null)
            {
                logger.LogWarning("File {Path} is not a valid module file.", path);
                continue;
            }

            if (module.Types == null || module.Types.Count == 0)
            {
                logger.LogWarning("Module {ModuleName} does not contain any types.", module.Name);
                continue;
            }

            dynamicTypes.AddRange(module.Types);
        }

        return dynamicTypes;
    }

    public IEnumerable<SitefinityType> GetSitefinityTypes()
    {
        string deploymentFolderPath = configuration.SitefinityModuleDeploymentFolderPath;

        if (!Path.IsPathRooted(configuration.SitefinityModuleDeploymentFolderPath))
        {
            deploymentFolderPath = Environment.CurrentDirectory + configuration.SitefinityModuleDeploymentFolderPath;
        }

        if (string.IsNullOrEmpty(deploymentFolderPath) || !Directory.Exists(deploymentFolderPath))
        {
            logger.LogError("Sitefinity module deployment folder does not exist. {DeploymentFolderPath}", deploymentFolderPath);
            return [];
        }

        var staticSitefinityTypes = GetStaticSitefinityTypes();

        var sitefinityTypes = new List<SitefinityType>();

        foreach (string sitefinityPath in sitefinityTypeDirectories)
        {
            foreach (string path in Directory.EnumerateFiles($"{deploymentFolderPath}\\{sitefinityPath}", "*.sf", SearchOption.AllDirectories)
                                            .Where(path => !Array.Exists(excludedFileNames, e => e.Equals(Path.GetFileName(path)))))
            {
                string fileContents = File.ReadAllText(path);

                if (string.IsNullOrEmpty(fileContents))
                {
                    logger.LogWarning("File {Path} is empty.", path);
                    continue;
                }

                var types = JsonSerializer.Deserialize<IEnumerable<StaticSitefinityType>>(fileContents);

                if (types == null)
                {
                    logger.LogWarning("File {Path} is not a valid type file.", path);
                    continue;
                }

                sitefinityTypes.AddRange(types);
            }
        }

        foreach (var sitefinityType in sitefinityTypes)
        {
            var existingType = staticSitefinityTypes.Find(x => x.Name == sitefinityType.Name);

            if (existingType != null && existingType.Fields != null && sitefinityType.Fields != null)
            {
                existingType.Fields.AddRange(sitefinityType.Fields);
            }
        }

        return staticSitefinityTypes;
    }

    private List<StaticSitefinityType> GetStaticSitefinityTypes()
    {
        if (!Directory.Exists(staticSitefinityTypesDirectory))
        {
            logger.LogInformation("Static Sitefinity types folder does not exist. {StaticSitefinityTypesDirectory}", staticSitefinityTypesDirectory);
            return [];
        }

        var staticTypes = new List<StaticSitefinityType>();

        foreach (string path in Directory.EnumerateFiles(staticSitefinityTypesDirectory, "*.json", SearchOption.AllDirectories))
        {
            string fileContents = File.ReadAllText(path);

            if (string.IsNullOrEmpty(fileContents))
            {
                logger.LogWarning("File {Path} is empty.", path);
                continue;
            }

            var type = JsonSerializer.Deserialize<IEnumerable<StaticSitefinityType>>(fileContents);

            if (type == null)
            {
                logger.LogWarning("File {Path} is not a valid static sitefinity file.", path);
                continue;
            }

            staticTypes.AddRange(type);
        }

        return staticTypes;
    }
}
