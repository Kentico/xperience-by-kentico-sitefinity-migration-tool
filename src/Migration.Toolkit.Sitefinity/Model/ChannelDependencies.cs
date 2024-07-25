using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Sitefinity.Core.Models;

namespace Migration.Toolkit.Sitefinity.Model;
/// <summary>
/// Model for dependencies required for channel import.
/// </summary>
internal class ChannelDependencies : IImportDependencies
{
    /// <summary>
    /// Required content languages for channels.
    /// </summary>
    public required IDictionary<Guid, ContentLanguageModel> ContentLanguages { get; set; }
}
