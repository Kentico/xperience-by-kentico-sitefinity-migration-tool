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
                                            IDataClassImportService dataClassImportService,
                                            IMediaImportService mediaImportService,
                                            IUserImportService userImportService,
                                            IContentItemImportService contentItemImportService,
                                            IContentProvider contentProvider,
                                            IUmtAdapterWithDependencies<Page, ContentDependencies, WebPageItemModel> adapter) : IWebPageImportService
    {
        public IEnumerable<WebPageItemModel> Get(ContentDependencies dependenciesModel)
        {
            var pages = contentProvider.GetPages();

            foreach (var page in pages)
            {
                Console.WriteLine(page.UrlName);
            }

            return adapter.Adapt(pages, dependenciesModel);
        }
        public SitefinityImportResult<WebPageItemModel> StartImport(ImportStateObserver observer)
        {
            var mediaFiles = mediaImportService.StartImport(observer);

            var users = userImportService.StartImport(mediaFiles.Observer);

            var dataClasses = dataClassImportService.StartImport(users.Observer);

            var dependencies = new ContentDependencies
            {
                MediaFiles = mediaFiles.ImportedModels,
                Users = users.ImportedModels,
                DataClasses = dataClasses.ImportedModels
            };

            var contentItems = contentItemImportService.StartImportWithDependencies(dataClasses.Observer, dependencies);

            dependencies.ContentItems = contentItems.ImportedModels;

            var pages = Get(dependencies);

            return new SitefinityImportResult<WebPageItemModel>
            {
                ImportedModels = pages.ToDictionary(x => x.WebPageItemGUID),
                Observer = observer// kenticoImportService.StartImport(pages, dataClasses.Observer)
            };
        }

        public SitefinityImportResult<WebPageItemModel> StartImportWithDependencies(ImportStateObserver observer, ContentDependencies dependenciesModel) => throw new NotImplementedException();
    }
}
