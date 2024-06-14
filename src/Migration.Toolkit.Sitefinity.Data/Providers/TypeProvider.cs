using System.Text.Json;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Configuration;
using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Data.Providers;
internal class TypeProvider(SitefinityDataConfiguration configuration, ILogger<TypeProvider> logger) : ITypeProvider
{
    private readonly string[] excludedFileNames = new string[] { "version.sf", "configs.sf", "widgetTemplates.sf" };
    private readonly string[] sitefinityTypeDirectories = new string[] { "Blogs", "Events", "Lists", "News" };
    private readonly string staticSitefinityTypesPath = Environment.CurrentDirectory + "\\SitefinityTypes.json";

    public IEnumerable<SitefinityType> GetAllTypes()
    {
        var sitefinityTypes = new List<SitefinityType>();

        sitefinityTypes.AddRange(GetDynamicModuleTypes());
        sitefinityTypes.AddRange(GetSitefinityTypes());

        return sitefinityTypes;
    }

    public IEnumerable<SitefinityType> GetDynamicModuleTypes()
    {
        string dynamicModulesPath = Environment.CurrentDirectory + configuration.SitefinityModuleDeploymentFolderPath + "\\Dynamic modules";
        if (!Directory.Exists(dynamicModulesPath))
        {
            logger.LogError($"Sitefinity module deployment folder does not exist. {dynamicModulesPath}");
            return [];
        }

        var dynamicTypes = new List<DynamicModuleType>();

        foreach (string path in Directory.EnumerateFiles(dynamicModulesPath, "*.sf", SearchOption.AllDirectories)
                                            .Where(path => !excludedFileNames.Any(e => e.Equals(Path.GetFileName(path)))))
        {
            string fileContents = File.ReadAllText(path);

            if (string.IsNullOrEmpty(fileContents))
            {
                logger.LogWarning($"File {path} is empty.");
                continue;
            }

            var module = JsonSerializer.Deserialize<Module>(fileContents);

            if (module == null)
            {
                logger.LogWarning($"File {path} is not a valid module file.");
                continue;
            }

            if (module.Types == null || module.Types.Count == 0)
            {
                logger.LogWarning($"Module {module.Name} does not contain any types.");
                continue;
            }

            dynamicTypes.AddRange(module.Types);
        }

        return dynamicTypes;
    }

    public IEnumerable<SitefinityType> GetSitefinityTypes()
    {
        string deploymentFolderPath = Environment.CurrentDirectory + configuration.SitefinityModuleDeploymentFolderPath;
        if (!Directory.Exists(deploymentFolderPath))
        {
            logger.LogError($"Sitefinity module deployment folder does not exist. {deploymentFolderPath}");
            return [];
        }

        var staticSitefinityTypes = GetStaticSitefinityTypes();

        var sitefinityTypes = new List<SitefinityType>();

        foreach (string sitefinityPath in sitefinityTypeDirectories)
        {
            foreach (string path in Directory.EnumerateFiles($"{deploymentFolderPath}\\{sitefinityPath}", "*.sf", SearchOption.AllDirectories)
                                            .Where(path => !excludedFileNames.Any(e => e.Equals(Path.GetFileName(path)))))
            {
                string fileContents = File.ReadAllText(path);

                if (string.IsNullOrEmpty(fileContents))
                {
                    logger.LogWarning($"File {path} is empty.");
                    continue;
                }

                var types = JsonSerializer.Deserialize<IEnumerable<StaticSitefinityType>>(fileContents);

                if (types == null)
                {
                    logger.LogWarning($"File {path} is not a valid type file.");
                    continue;
                }

                sitefinityTypes.AddRange(types);
            }
        }

        foreach (var sitefinityType in sitefinityTypes)
        {
            var existingType = staticSitefinityTypes.FirstOrDefault(x => x.Name == sitefinityType.Name);

            if (existingType != null && existingType.Fields != null && sitefinityType.Fields != null)
            {
                existingType.Fields.AddRange(sitefinityType.Fields);
            }
        }

        return staticSitefinityTypes;
    }

    private IEnumerable<SitefinityType> GetStaticSitefinityTypes()
    {
        if (!File.Exists(staticSitefinityTypesPath))
        {
            logger.LogInformation($"Static Sitefinity types file does not exist. {staticSitefinityTypesPath}");
            return [];
        }

        string fileContents = File.ReadAllText(staticSitefinityTypesPath);

        if (string.IsNullOrEmpty(fileContents))
        {
            logger.LogWarning($"File {staticSitefinityTypesPath} is empty.");
            return [];
        }

        var types = JsonSerializer.Deserialize<IEnumerable<StaticSitefinityType>>(fileContents);

        if (types == null)
        {
            logger.LogWarning($"File {staticSitefinityTypesPath} is not a valid type file.");
            return [];
        }

        return types;
    }
}
