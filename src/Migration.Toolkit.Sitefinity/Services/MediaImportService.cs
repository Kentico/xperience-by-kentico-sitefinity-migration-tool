using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core.Adapters;
using Migration.Toolkit.Sitefinity.Core.Services;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Services;

internal class MediaImportService(IImportService kenticoImportService,
                                    IContentLanguageImportService contentLanguageImportService,
                                    IUserImportService userImportService,
                                    IDataClassImportService dataClassImportService,
                                    IContentFolderImportService contentFolderImportService,
                                    ContentFolderManager folderManager,
                                    IMediaProvider mediaProvider,
                                    IUmtAdapterWithDependencies<Media, MediaFileDependencies> adapter) : IMediaImportService
{
    /// <summary>
    /// Cached media files to avoid multiple provider calls.
    /// </summary>
    private List<Media>? cachedMediaFiles;

    /// <summary>
    /// Gets all media files from the provider, cached after first call to avoid redundant database/API calls.
    /// </summary>
    /// <returns>List of all media files (images, documents, videos)</returns>
    private List<Media> GetAllMediaFiles()
    {
        if (cachedMediaFiles == null)
        {
            cachedMediaFiles = [];
            cachedMediaFiles.AddRange(mediaProvider.GetImages());
            cachedMediaFiles.AddRange(mediaProvider.GetDocuments());
            cachedMediaFiles.AddRange(mediaProvider.GetVideos());
        }

        return cachedMediaFiles;
    }

    public IEnumerable<ContentItemSimplifiedModel> Get(MediaFileDependencies dependenciesModel)
    {
        var allMediaFiles = GetAllMediaFiles();
        return adapter.Adapt(allMediaFiles, dependenciesModel).OfType<ContentItemSimplifiedModel>();
    }

    public SitefinityImportResult<ContentItemSimplifiedModel> StartImport(ImportStateObserver observer)
    {
        var contentLanguageResults = contentLanguageImportService.StartImport(observer);
        observer.ImportCompletedTask.Wait();

        var userImportResults = userImportService.StartImport(observer);
        observer.ImportCompletedTask.Wait();

        var dataClassResults = dataClassImportService.StartImport(observer);
        observer.ImportCompletedTask.Wait();

        var mediaFileDependencies = new MediaFileDependencies
        {
            ContentFolders = new Dictionary<Guid, ContentFolderModel>(),
            Users = userImportResults.ImportedModels,
            DataClasses = dataClassResults.ImportedModels.Values.OfType<DataClassModel>().ToDictionary(x => x.ClassGUID),
            ContentLanguages = contentLanguageResults.ImportedModels,
            FolderManager = folderManager
        };

        return ImportMediaWithFolders(observer, mediaFileDependencies);
    }

    public SitefinityImportResult<ContentItemSimplifiedModel> StartImportWithDependencies(ImportStateObserver observer, MediaFileDependencies dependenciesModel) => ImportMediaWithFolders(observer, dependenciesModel);

    private SitefinityImportResult<ContentItemSimplifiedModel> ImportMediaWithFolders(ImportStateObserver observer, MediaFileDependencies mediaFileDependencies)
    {
        // Clear any previous folder tracking and pre-process media to create folder structure
        folderManager.ClearCreatedFolders();

        // Get all media files once (cached for subsequent calls)
        var allMediaFiles = GetAllMediaFiles();

        // Run adapter to trigger folder creation (results will be discarded, we just want folders)
        _ = adapter.Adapt(allMediaFiles, mediaFileDependencies).OfType<ContentItemSimplifiedModel>().ToList();

        // Import all dynamically created folders in hierarchical order
        var allCreatedFolders = folderManager.GetAllCreatedFolders();

        if (allCreatedFolders.Any())
        {
            var folderDependencies = new ContentFolderDependencies
            {
                ContentFolders = mediaFileDependencies.ContentFolders
            };
            contentFolderImportService.ImportFoldersHierarchically(allCreatedFolders, folderDependencies, observer);
        }

        // Now import media items using the updated dependencies that include all folders
        return ImportMediaWithUpdatedDependencies(observer, mediaFileDependencies);
    }

    private SitefinityImportResult<ContentItemSimplifiedModel> ImportMediaWithUpdatedDependencies(ImportStateObserver observer, MediaFileDependencies mediaDependencies)
    {
        // Import media items using the updated dependencies that now include all created folders
        var importedMediaFiles = Get(mediaDependencies);

        return new SitefinityImportResult<ContentItemSimplifiedModel>
        {
            ImportedModels = importedMediaFiles.ToDictionary(x => x.ContentItemGUID),
            Observer = kenticoImportService.StartImport(importedMediaFiles, observer)
        };
    }
}
