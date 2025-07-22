using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Sitefinity.Core.Models;

namespace Migration.Toolkit.Sitefinity.Model;

/// <summary>
/// Model for dependencies required for content folder import.
/// </summary>
internal class ContentFolderDependencies : IImportDependencies
{
    /// <summary>
    /// Dictionary of existing content folders by GUID.
    /// </summary>
    public required IDictionary<Guid, ContentFolderModel> ContentFolders { get; set; }
}
