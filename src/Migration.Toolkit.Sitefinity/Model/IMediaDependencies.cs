using Kentico.Xperience.UMT.Model;

namespace Migration.Toolkit.Sitefinity.Model;
internal interface IMediaDependencies
{
    public IDictionary<Guid, MediaFileModel> MediaFiles { get; set; }
}
