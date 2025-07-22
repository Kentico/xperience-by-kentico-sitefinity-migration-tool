using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Sitefinity.Core.Models;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Core.Services;

/// <summary>
/// Service for importing content folders.
/// </summary>
internal interface IContentFolderImportService : IDataImportServiceWithDependencies<ContentFolderDependencies, ContentFolderModel>
{
    /// <summary>
    /// Imports folders hierarchically in the correct order (parent folders before child folders).
    /// </summary>
    /// <param name="folders">Folders to import</param>
    /// <param name="dependencies">Dependency objects</param>
    /// <param name="observer">Import observer</param>
    void ImportFoldersHierarchically(IEnumerable<ContentFolderModel> folders, ContentFolderDependencies dependencies, ImportStateObserver observer);
}
