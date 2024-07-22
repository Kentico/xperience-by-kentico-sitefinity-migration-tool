using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Sitefinity.Core.Models;

namespace Migration.Toolkit.Sitefinity.Model;
/// <summary>
/// Model for dependencies required for data class import.
/// </summary>
internal class DataClassDependencies : IImportDependencies
{
    /// <summary>
    /// Required channels for data classes.
    /// </summary>
    public required IDictionary<Guid, ChannelModel> Channels { get; set; }
}
