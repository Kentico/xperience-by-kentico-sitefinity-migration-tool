using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Tookit.Data.Models;
using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Sitefinity.Core.Adapters;
using Migration.Toolkit.Sitefinity.Core.Services;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Services;
internal class MediaImportService(IImportService kenticoImportService,
                                    IMediaLibraryImportService mediaLibraryImportService,
                                            IMediaProvider mediaProvider,
                                            IUmtAdapter<Media, MediaFileModel> adapter) : IMediaImportService
{
    public IEnumerable<MediaFileModel> Get(MediaFileDependencies dependenciesModel)
    {
        var mediaFiles = new List<Media>();

        mediaFiles.AddRange(mediaProvider.GetImages());

        mediaFiles.AddRange(mediaProvider.GetDocuments());

        mediaFiles.AddRange(mediaProvider.GetVideos());

        return adapter.Adapt(mediaFiles);
    }

    public SitefinityImportResult<MediaFileModel> StartImport(ImportStateObserver observer)
    {
        var result = mediaLibraryImportService.StartImport(observer);

        var dependencies = new MediaFileDependencies
        {
            MediaLibraries = result.ImportedModels
        };

        var mediaFiles = Get(dependencies);

        return new SitefinityImportResult<MediaFileModel>
        {
            ImportedModels = mediaFiles,
            Observer = kenticoImportService.StartImport(mediaFiles, result.Observer)
        };

        return kenticoImportService.StartImport(mediaFiles);
    }
}
