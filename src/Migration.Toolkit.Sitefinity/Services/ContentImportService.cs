using CMS.Helpers;

using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core.Adapters;
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
                                            IUmtAdapterWithDependencies<ContentItem, ContentDependencies, ContentItemSimplifiedModel> adapter) : IContentImportService
    {
        public IEnumerable<ContentItemSimplifiedModel> Get(ContentDependencies dependenciesModel)
        {
            var typeDefinitions = new List<SitefinityTypeDefinition>();

            foreach (var dataClass in dependenciesModel.DataClasses.Select(x => x.Value))
            {
                var types = typeProvider.GetAllTypes();

                var type = types.FirstOrDefault(x => x.Id == dataClass.ClassGUID);

                if (type == null)
                {
                    continue;
                }

                string typeName = $"{type.ClassNamespace}.{type.Name}";

                var dataClassGuid = ValidationHelper.GetGuid(dataClass.ClassGUID, Guid.Empty);

                if (dataClassGuid.Equals(Guid.Empty))
                {
                    continue;
                }

                typeDefinitions.Add(new SitefinityTypeDefinition
                {
                    SitefinityTypeName = typeName,
                    DataClassGuid = dataClassGuid,
                });
            }

            var contentItems = contentProvider.GetContentItems(typeDefinitions, dependenciesModel.ContentLanguages.Select(x => x.Value).Select(z => z.ContentLanguageCultureFormat));

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

            var contentItems = Get(dependencies);

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
