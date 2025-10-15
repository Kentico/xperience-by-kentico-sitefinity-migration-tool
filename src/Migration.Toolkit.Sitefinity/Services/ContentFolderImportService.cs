using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Sitefinity.Core.Services;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Services;

/// <summary>
/// Service for importing content folders.
/// </summary>
internal class ContentFolderImportService(IImportService kenticoImportService,
                                          ContentFolderManager folderManager,
                                          ILogger<ContentFolderImportService> logger) : IContentFolderImportService
{
    public IEnumerable<ContentFolderModel> Get(ContentFolderDependencies dependenciesModel) => folderManager.GetAllCreatedFolders();

    public SitefinityImportResult<ContentFolderModel> StartImport(ImportStateObserver observer)
    {
        // For standalone import, we don't have any folders to import by default
        // This method would be used if folders need to be imported independently
        var dependencies = new ContentFolderDependencies
        {
            ContentFolders = new Dictionary<Guid, ContentFolderModel>()
        };

        return ImportFolders(observer, dependencies);
    }

    public SitefinityImportResult<ContentFolderModel> StartImportWithDependencies(ImportStateObserver observer, ContentFolderDependencies dependenciesModel) => ImportFolders(observer, dependenciesModel);

    public void ImportFoldersHierarchically(IEnumerable<ContentFolderModel> folders, ContentFolderDependencies dependencies, ImportStateObserver observer)
    {
        var foldersList = folders.ToList();
        if (foldersList.Count == 0)
        {
            logger.LogInformation("No folders to import");
            return;
        }

        // Group folders by their depth level
        var foldersByLevel = foldersList
            .GroupBy(folderItem => folderItem.ContentFolderTreePath?.Split('/', StringSplitOptions.RemoveEmptyEntries).Length ?? 0)
            .OrderBy(levelGroup => levelGroup.Key)
            .ToList();

        logger.LogInformation("Importing {FolderCount} folders across {LevelCount} levels", foldersList.Count, foldersByLevel.Count);

        // Import each level sequentially
        foreach (var currentLevelGroup in foldersByLevel)
        {
            var foldersAtCurrentLevel = currentLevelGroup.ToList();
            logger.LogDebug("Importing {FolderCount} folders at level {Level}", foldersAtCurrentLevel.Count, currentLevelGroup.Key);

            // Import all folders at this level
            kenticoImportService.StartImport(foldersAtCurrentLevel, observer);
            observer.ImportCompletedTask.Wait();

            // Add the imported folders to dependencies so next level can reference them
            foreach (var importedFolder in foldersAtCurrentLevel)
            {
                if (importedFolder.ContentFolderGUID.HasValue)
                {
                    dependencies.ContentFolders[importedFolder.ContentFolderGUID.Value] = importedFolder;
                }
            }
        }

        logger.LogInformation("Completed importing all folders hierarchically");
    }

    private SitefinityImportResult<ContentFolderModel> ImportFolders(ImportStateObserver observer, ContentFolderDependencies dependencies)
    {
        var foldersToImport = Get(dependencies);
        var importedFolders = foldersToImport.ToDictionary(x => x.ContentFolderGUID ?? Guid.Empty);

        if (importedFolders.Count != 0)
        {
            ImportFoldersHierarchically(foldersToImport, dependencies, observer);
        }

        return new SitefinityImportResult<ContentFolderModel>
        {
            ImportedModels = importedFolders,
            Observer = observer
        };
    }
}
