using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core.Adapters;
using Migration.Toolkit.Sitefinity.Core.Services;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Services;
internal class MediaLibraryImportService(IImportService kenticoImportService,
                                            IMediaProvider mediaProvider,
                                            IUmtAdapter<Library, MediaLibraryModel> adapter) : IMediaLibraryImportService
{
    public IEnumerable<MediaLibraryModel> Get()
    {
        var libraries = new List<Library>();

        libraries.AddRange(mediaProvider.GetDocumentLibraries());

        libraries.AddRange(mediaProvider.GetImageLibraries());

        libraries.AddRange(mediaProvider.GetVideoLibraries());

        return adapter.Adapt(libraries);
    }

    public SitefinityImportResult<MediaLibraryModel> StartImport(ImportStateObserver observer)
    {
        var importedModels = Get();

        return new SitefinityImportResult<MediaLibraryModel>
        {
            ImportedModels = importedModels.ToDictionary(x => x.LibraryGUID),
            Observer = kenticoImportService.StartImport(importedModels, observer)
        };
    }
}
