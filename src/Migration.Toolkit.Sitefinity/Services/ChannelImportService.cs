using CMS.Helpers;

using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core.Adapters;
using Migration.Toolkit.Sitefinity.Core.Services;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Services;

internal class ChannelImportService(IImportService kenticoImportService,
                                        IContentLanguageImportService contentLanguageImportService,
                                        ISiteProvider siteProvider,
                                        IUmtAdapterWithDependencies<Site, ChannelDependencies> adapter) : IChannelImportService
{
    public IEnumerable<IUmtModel> Get(ChannelDependencies dependenciesModel)
    {
        var sites = siteProvider.GetSites();

        return adapter.Adapt(sites, dependenciesModel);
    }

    public SitefinityImportResult StartImport(ImportStateObserver observer)
    {
        var languages = contentLanguageImportService.StartImport(observer);

        observer.ImportCompletedTask.Wait();

        var channelDependencies = new ChannelDependencies
        {
            ContentLanguages = languages.ImportedModels
        };

        return Import(observer, channelDependencies);
    }

    public SitefinityImportResult StartImportWithDependencies(ImportStateObserver observer, ChannelDependencies dependenciesModel) => Import(observer, dependenciesModel);

    private SitefinityImportResult Import(ImportStateObserver observer, ChannelDependencies dependenciesModel)
    {
        var channels = Get(dependenciesModel);

        var importedModels = new Dictionary<Guid, IUmtModel>();

        foreach (var channel in channels.OfType<ChannelModel>())
        {
            var guid = ValidationHelper.GetGuid(channel.ChannelGUID, Guid.Empty);

            if (guid.Equals(Guid.Empty))
            {
                continue;
            }

            importedModels.Add(guid, channel);
        }

        return new SitefinityImportResult
        {
            ImportedModels = importedModels,
            Observer = kenticoImportService.StartImport(channels, observer)
        };
    }
}
