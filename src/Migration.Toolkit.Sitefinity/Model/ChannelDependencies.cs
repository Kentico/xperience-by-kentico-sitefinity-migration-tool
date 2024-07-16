using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Sitefinity.Core.Models;

namespace Migration.Toolkit.Sitefinity.Model;
internal class ChannelDependencies : IImportDependencies
{
    public required IDictionary<Guid, ContentLanguageModel> ContentLanguages { get; set; }
}
