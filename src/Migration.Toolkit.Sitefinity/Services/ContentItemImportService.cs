using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Configuration;
using Migration.Toolkit.Sitefinity.Core.Adapters;
using Migration.Toolkit.Sitefinity.Core.Helpers;
using Migration.Toolkit.Sitefinity.Core.Services;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Services
{
    internal class ContentItemImportService(IImportService kenticoImportService,
                                            IContentLanguageImportService contentLanguageImportService,
                                            IChannelImportService channelImportService,
                                            IDataClassImportService dataClassImportService,
                                            IMediaImportService mediaImportService,
                                            IUserImportService userImportService,
                                            IWebPageImportService webPageImportService,
                                            IContentProvider contentProvider,
                                            ITypeProvider typeProvider,
                                            IContentHelper contentHelper,
                                            SitefinityImportConfiguration importConfiguration,
                                            ILogger<ContentItemImportService> logger,
                                            IUmtAdapterWithDependencies<ContentItem, ContentDependencies, ContentItemSimplifiedModel> adapter) : IContentItemImportService
    {
        public IEnumerable<ContentItemSimplifiedModel> Get(ContentDependencies dependenciesModel)
        {
            var typeDefinitions = new List<SitefinityTypeDefinition>();

            foreach (var dataClassGuid in dependenciesModel.DataClasses.Keys)
            {
                var types = typeProvider.GetAllTypes().Where(type => Array.Exists(Constants.ForcedWebsiteTypes, x => !x.Equals(type.Name)));

                var type = types.FirstOrDefault(x => x.Id == dataClassGuid);

                if (type == null)
                {
                    logger.LogWarning("No type found for dataclass with ClassGuid of {DataClassGuid}. Cannot get items based on data class: {DataClassName}", dataClassGuid, dependenciesModel.DataClasses[dataClassGuid].ClassName);
                    continue;
                }

                if (type.ClassNamespace == null || type.Name == null)
                {
                    continue;
                }

                typeDefinitions.Add(new SitefinityTypeDefinition
                {
                    SitefinityTypeNameSpace = type.ClassNamespace,
                    SitefinityTypeName = type.Name,
                    DataClassGuid = dataClassGuid,
                });
            }

            var channel = contentHelper.GetCurrentChannel(dependenciesModel.Channels.Values);
            var currentSite = contentHelper.GetCurrentSite();

            if (channel == null || currentSite == null)
            {
                logger.LogWarning("Channel/Site not found. Cannot import content items.");
                return [];
            }

            var detailPageConfigs = importConfiguration.PageContentTypes?.Where(x => x.PageTemplateType == PageTemplateType.Detail);

            var contentItems = contentProvider.GetContentItems(typeDefinitions, currentSite.SystemCultures).OrderByDescending(x => (detailPageConfigs?.Any(z => z.TypeName.Equals(x.TypeName)) ?? false) ? x.TypeName : "");

            return adapter.Adapt(contentItems, dependenciesModel);
        }
        public SitefinityImportResult<ContentItemSimplifiedModel> StartImport(ImportStateObserver observer)
        {
            var languages = contentLanguageImportService.StartImport(observer);

            observer.ImportCompletedTask.Wait();

            var channelDependencies = new ChannelDependencies
            {
                ContentLanguages = languages.ImportedModels
            };

            var channels = channelImportService.StartImportWithDependencies(observer, channelDependencies);

            observer.ImportCompletedTask.Wait();

            var users = userImportService.StartImport(observer);

            observer.ImportCompletedTask.Wait();

            var dataClassDependencies = new DataClassDependencies
            {
                Channels = channels.ImportedModels.Values.OfType<ChannelModel>().ToDictionary(x => x.ChannelGUID)
            };

            var dataClasses = dataClassImportService.StartImportWithDependencies(observer, dataClassDependencies);

            observer.ImportCompletedTask.Wait();


            var mediaFiles = mediaImportService.StartImport(observer);

            observer.ImportCompletedTask.Wait();

            var dependencies = new ContentDependencies
            {
                MediaFiles = mediaFiles.ImportedModels,
                Users = users.ImportedModels,
                DataClasses = dataClasses.ImportedModels.Values.OfType<DataClassModel>().ToDictionary(x => x.ClassGUID),
                Channels = channels.ImportedModels.Values.OfType<ChannelModel>().ToDictionary(x => x.ChannelGUID),
                ContentLanguages = languages.ImportedModels
            };

            var webpages = webPageImportService.StartImportWithDependencies(observer, dependencies);

            observer.ImportCompletedTask.Wait();

            dependencies.WebPages = webpages.ImportedModels;

            var contentItems = Get(dependencies).OrderBy(x => x.PageData == null ? "" : x.PageData.TreePath);

            return new SitefinityImportResult<ContentItemSimplifiedModel>
            {
                ImportedModels = contentItems.ToDictionary(x => x.ContentItemGUID),
                Observer = kenticoImportService.StartImport(contentItems, observer)
            };
        }
        public SitefinityImportResult<ContentItemSimplifiedModel> StartImportWithDependencies(ImportStateObserver observer, ContentDependencies dependenciesModel)
        {
            var contentItems = Get(dependenciesModel);

            return new SitefinityImportResult<ContentItemSimplifiedModel>
            {
                ImportedModels = contentItems.ToDictionary(x => x.ContentItemGUID),
                Observer = kenticoImportService.StartImport(contentItems, observer)
            };
        }
    }
}
