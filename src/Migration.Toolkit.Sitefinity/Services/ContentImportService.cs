using CMS.Helpers;

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
    internal class ContentImportService(IImportService kenticoImportService,
                                            IContentLanguageImportService contentLanguageImportService,
                                            IChannelImportService channelImportService,
                                            IDataClassImportService dataClassImportService,
                                            IMediaLibraryImportService mediaLibraryImportService,
                                            IMediaImportService mediaImportService,
                                            IUserImportService userImportService,
                                            IWebPageImportService webPageImportService,
                                            IContentProvider contentProvider,
                                            ITypeProvider typeProvider,
                                            IContentHelper contentHelper,
                                            ISiteProvider siteProvider,
                                            SitefinityImportConfiguration importConfiguration,
                                            ILogger<ContentImportService> logger,
                                            IUmtAdapterWithDependencies<ContentItem, ContentDependencies, ContentItemSimplifiedModel> adapter) : IContentImportService
    {
        public IEnumerable<ContentItemSimplifiedModel> Get(ContentDependencies dependenciesModel)
        {
            var typeDefinitions = new List<SitefinityTypeDefinition>();

            foreach (var dataClass in dependenciesModel.DataClasses.Select(x => x.Value))
            {
                var types = typeProvider.GetAllTypes().Where(type => Array.Exists(Constants.ForcedWebsiteTypes, x => !x.Equals(type.Name)));

                var type = types.FirstOrDefault(x => x.Id == dataClass.ClassGUID);

                if (type == null)
                {
                    continue;
                }
                var dataClassGuid = ValidationHelper.GetGuid(dataClass.ClassGUID, Guid.Empty);

                if (dataClassGuid.Equals(Guid.Empty))
                {
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
            if (channel == null)
            {
                logger.LogWarning("Channel not found. Cannot import content items.");
                return [];
            }

            var currentSite = siteProvider.GetSites().First(x => x.Id.Equals(channel.ChannelGUID));

            var detailPageConfigs = importConfiguration.PageContentTypes?.Where(x => x.PageTemplateType.Equals("Detail"));

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

            var mediaLibraries = mediaLibraryImportService.StartImport(observer);

            observer.ImportCompletedTask.Wait();

            var mediaFilesDependencies = new MediaFileDependencies
            {
                MediaLibraries = mediaLibraries.ImportedModels,
                Users = users.ImportedModels
            };

            var mediaFiles = mediaImportService.StartImportWithDependencies(observer, mediaFilesDependencies);

            observer.ImportCompletedTask.Wait();

            var dataClassDependencies = new DataClassDependencies
            {
                Channels = channels.ImportedModels.Values.OfType<ChannelModel>().ToDictionary(x => x.ChannelGUID)
            };

            var dataClasses = dataClassImportService.StartImportWithDependencies(observer, dataClassDependencies);

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
