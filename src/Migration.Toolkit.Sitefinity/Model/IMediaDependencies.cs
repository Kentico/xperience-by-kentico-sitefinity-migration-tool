using Kentico.Xperience.UMT.Model;

namespace Migration.Toolkit.Sitefinity.Model;
internal interface IMediaDependencies
{
    public IDictionary<Guid, ContentItemSimplifiedModel> MediaFiles { get; set; }
}
