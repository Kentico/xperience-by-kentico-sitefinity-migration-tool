using System.Text.Json;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Configuration;
using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Data.Providers;
internal class TypeProvider(SitefinityDataConfiguration configuration, ILogger<TypeProvider> logger) : ITypeProvider
{
    private readonly string[] excludedFileNames = ["version.sf", "configs.sf", "widgetTemplates.sf"];
    private readonly string[] sitefinityTypeDirectories = ["Blogs", "Events", "Lists", "News"];
    private readonly string staticSitefinityTypesDirectory = Environment.CurrentDirectory + "\\StaticSitefinityTypes";

    public IEnumerable<SitefinityType> GetAllTypes()
    {
        var sitefinityTypes = new List<SitefinityType>();

        sitefinityTypes.AddRange(GetDynamicModuleTypes());
        sitefinityTypes.AddRange(GetSitefinityTypes());

        return sitefinityTypes;
    }

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
