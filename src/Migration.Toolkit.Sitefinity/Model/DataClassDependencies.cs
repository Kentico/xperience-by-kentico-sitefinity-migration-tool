using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Sitefinity.Core.Models;

namespace Migration.Toolkit.Sitefinity.Model;
internal class DataClassDependencies : IImportDependencies
{
    public required IDictionary<Guid, ChannelModel> Channels { get; set; }
}
