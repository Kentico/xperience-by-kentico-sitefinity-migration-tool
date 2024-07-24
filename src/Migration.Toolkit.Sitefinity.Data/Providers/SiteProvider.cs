using System.Text.Json;
using System.Xml.Linq;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Configuration;
using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Data.Providers;
internal class SiteProvider(SitefinityDataConfiguration configuration, ILogger<SiteProvider> logger) : ISiteProvider
{
    public IEnumerable<Site> GetSites()
    {
        string siteFolder = configuration.SitefinityModuleDeploymentFolderPath + "\\Multisite\\Content";

        if (!Path.IsPathRooted(configuration.SitefinityModuleDeploymentFolderPath))
        {
            siteFolder = Environment.CurrentDirectory + configuration.SitefinityModuleDeploymentFolderPath + "\\Multisite\\Content";
        }

        if (string.IsNullOrEmpty(siteFolder) || !Directory.Exists(siteFolder))
        {
            logger.LogError("Sitefinity site folder does not exist. {SiteFolder}", siteFolder);
            return [];
        }

        var sites = new List<Site>();

        foreach (string path in Directory.EnumerateFiles(siteFolder, "*.xml", SearchOption.AllDirectories))
        {
            string fileContents = File.ReadAllText(path);

            if (string.IsNullOrEmpty(fileContents))
            {
                logger.LogWarning("File {Path} is empty.", path);
                continue;
            }

            var xmlDoc = XDocument.Load(path);
            XNamespace cmisra = "http://docs.oasis-open.org/ns/cmis/restatom/200908/";

            var cmisraObjects = xmlDoc.Descendants(cmisra + "object");

            foreach (var obj in cmisraObjects)
            {
                XNamespace cmis = "http://docs.oasis-open.org/ns/cmis/core/200908/";
                var title = obj.Descendants(cmis + "propertyString")
                                            .FirstOrDefault(ps => ps.Attribute("propertyDefinitionId")?.Value == "sf:Name");


                var siteDefinition = obj.Descendants(cmis + "propertyString")
                                            .FirstOrDefault(ps => ps.Attribute("propertyDefinitionId")?.Value == "sf:SiteConfigurationViewModelProp");

                if (siteDefinition == null)
                {
                    logger.LogWarning("Site definition not found for site: {SiteName}", title?.Name);
                    continue;
                }

                string? jsonValue = siteDefinition.Element(cmis + "value")?.Value;

                if (jsonValue == null)
                {
                    logger.LogWarning("Site definition is empty for site: {SiteName}", title?.Name);
                    continue;
                }

                var site = JsonSerializer.Deserialize<Site>(jsonValue);

                if (site == null)
                {
                    logger.LogWarning("Site definition is not a valid JSON for site: {SiteName}", title?.Name);
                    continue;
                }

                sites.Add(site);
            }
        }

        return sites;
    }
}
