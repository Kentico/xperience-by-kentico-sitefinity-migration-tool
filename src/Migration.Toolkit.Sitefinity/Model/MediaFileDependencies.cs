using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Sitefinity.Core.Models;

namespace Migration.Toolkit.Sitefinity.Model;
/// <summary>
/// Model for dependencies required for media file import.
/// </summary>
internal class MediaFileDependencies : IImportDependencies
{
    /// <summary>
    /// Required media libraries for media files.
    /// </summary>
    public required IDictionary<Guid, MediaLibraryModel> MediaLibraries { get; set; }


    /// <summary>
    /// Required users for media files.
    /// </summary>
    public required IDictionary<Guid, UserInfoModel> Users { get; set; }
}
