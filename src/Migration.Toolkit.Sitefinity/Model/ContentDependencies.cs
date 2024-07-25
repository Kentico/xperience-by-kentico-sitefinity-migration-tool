using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Sitefinity.Core.Models;

namespace Migration.Toolkit.Sitefinity.Model;
internal class ContentDependencies : IImportDependencies
{
    /// <summary>
    /// Required media files for content.
    /// </summary>
    public required IDictionary<Guid, MediaFileModel> MediaFiles { get; set; }
    /// <summary>
    /// Required users for content.
    /// </summary>
    public required IDictionary<Guid, UserInfoModel> Users { get; set; }
    /// <summary>
    /// Required data classes for content.
    /// </summary>
    public required IDictionary<Guid, DataClassModel> DataClasses { get; set; }
    /// <summary>
    /// Required content languages for content.
    /// </summary>
    public required IDictionary<Guid, ContentLanguageModel> ContentLanguages { get; set; }
    /// <summary>
    /// Required channels for content.
    /// </summary>
    public required IDictionary<Guid, ChannelModel> Channels { get; set; }
    /// <summary>
    /// Imported web pages
    /// </summary>
    public IDictionary<Guid, ContentItemSimplifiedModel>? WebPages { get; set; }
}
