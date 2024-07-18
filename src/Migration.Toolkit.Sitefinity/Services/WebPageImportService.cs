using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core.Adapters;
using Migration.Toolkit.Sitefinity.Core.Services;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Services
{
    internal class WebPageImportService(IImportService kenticoImportService,
                                            IContentLanguageImportService contentLanguageImportService,
                                            IChannelImportService channelImportService,
                                            IDataClassImportService dataClassImportService,
                                            IMediaLibraryImportService mediaLibraryImportService,
                                            IMediaImportService mediaImportService,
                                            IUserImportService userImportService,
                                            IContentProvider contentProvider,
                                            IUmtAdapterWithDependencies<Page, ContentDependencies, ContentItemSimplifiedModel> adapter) : IWebPageImportService
    {
        public IEnumerable<ContentItemSimplifiedModel> Get(ContentDependencies dependenciesModel)
        {
            var pages = contentProvider.GetPages();

            return adapter.Adapt(pages, dependenciesModel);
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

            return Import(observer, dependencies);
        }

        public SitefinityImportResult<ContentItemSimplifiedModel> StartImportWithDependencies(ImportStateObserver observer, ContentDependencies dependenciesModel) => Import(observer, dependenciesModel);

        private SitefinityImportResult<ContentItemSimplifiedModel> Import(ImportStateObserver observer, ContentDependencies dependencies)
        {
            var pages = Get(dependencies);

            return new SitefinityImportResult<ContentItemSimplifiedModel>
            {
                ImportedModels = pages.ToDictionary(x => x.ContentItemGUID),
                Observer = kenticoImportService.StartImport(pages, observer)
            };
        }
    }
}
