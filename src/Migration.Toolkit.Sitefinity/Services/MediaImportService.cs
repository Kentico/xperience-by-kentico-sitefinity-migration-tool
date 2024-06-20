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
                                    IUserImportService userImportService,
                                            IMediaProvider mediaProvider,
                                            IUmtAdapter<Media, MediaFileDependencies, MediaFileModel> adapter) : IMediaImportService
{
    public IEnumerable<MediaFileModel> Get(MediaFileDependencies dependenciesModel)
    {
        var mediaFiles = new List<Media>();

        mediaFiles.AddRange(mediaProvider.GetImages());

        mediaFiles.AddRange(mediaProvider.GetDocuments());

        mediaFiles.AddRange(mediaProvider.GetVideos());

        return adapter.Adapt(mediaFiles, dependenciesModel);
    }

    public SitefinityImportResult<MediaFileModel> StartImport(ImportStateObserver observer)
    {
        var mediaLibraryResult = mediaLibraryImportService.StartImport(observer);
        var userResult = userImportService.StartImport(mediaLibraryResult.Observer);

        var dependencies = new MediaFileDependencies
        {
            MediaLibraries = mediaLibraryResult.ImportedModels,
            Users = userResult.ImportedModels
        };

        var mediaFiles = Get(dependencies);

        return new SitefinityImportResult<MediaFileModel>
        {
            ImportedModels = mediaFiles.ToDictionary(x => x.FileGUID),
            Observer = kenticoImportService.StartImport(mediaFiles, userResult.Observer)
        };
    }
}
