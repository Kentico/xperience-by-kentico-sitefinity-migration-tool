using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core.Adapters;
using Migration.Toolkit.Sitefinity.Core.Services;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Services;

internal class ContentLanguageImportService(IImportService kenticoImportService,
                                                ISiteProvider siteProvider,
                                                IUmtAdapter<SystemCulture, ContentLanguageModel> adapter) : IContentLanguageImportService
{
    public IEnumerable<ContentLanguageModel> Get()
    {
        var sites = siteProvider.GetSites();

        var cultures = new Dictionary<string, SystemCulture>();

        foreach (var siteCulture in sites.Select(x => x.SystemCultures))
        {
            if (siteCulture == null)
            {
                continue;
            }

            foreach (var culture in siteCulture)
            {
                if (culture.Key == null || cultures.ContainsKey(culture.Key))
                {
                    continue;
                }

                cultures.Add(culture.Key, culture);
            }
        }

        return adapter.Adapt(cultures.Values);
    }
    public SitefinityImportResult<ContentLanguageModel> StartImport(ImportStateObserver observer)
    {
        var languages = Get();

        return new SitefinityImportResult<ContentLanguageModel>
        {
            ImportedModels = languages.ToDictionary(x => x.ContentLanguageGUID),
            Observer = kenticoImportService.StartImport(languages, observer)
        };
    }
}
