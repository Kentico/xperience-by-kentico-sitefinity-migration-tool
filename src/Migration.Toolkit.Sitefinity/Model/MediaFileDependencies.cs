using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Sitefinity.Core.Models;
using Migration.Toolkit.Sitefinity.Services;

namespace Migration.Toolkit.Sitefinity.Model;
/// <summary>
/// Model for dependencies required for media file import.
/// </summary>
internal class MediaFileDependencies : IImportDependencies
{
    /// <summary>
    /// Required content folders for media files.
    /// </summary>
    public required IDictionary<Guid, ContentFolderModel> ContentFolders { get; set; }


    /// <summary>
    /// Required users for media files.
    /// </summary>
    public required IDictionary<Guid, UserInfoModel> Users { get; set; }

    /// <summary>
    /// Required data classes for media content types.
    /// </summary>
    public required IDictionary<Guid, DataClassModel> DataClasses { get; set; }

    /// <summary>
    /// Required content languages for media content items.
    /// </summary>
    public required IDictionary<Guid, ContentLanguageModel> ContentLanguages { get; set; }

    /// <summary>
    /// Content folder manager for creating and tracking folders.
    /// </summary>
    public required ContentFolderManager FolderManager { get; set; }
}
