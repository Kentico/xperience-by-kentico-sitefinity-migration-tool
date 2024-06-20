using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Sitefinity.Core.Models;

namespace Migration.Toolkit.Sitefinity.Model
{
    internal class MediaFileDependencies : IImportDependencies
    {
        public required IDictionary<Guid, MediaLibraryModel> MediaLibraries { get; set; }
        public required IDictionary<Guid, UserInfoModel> Users { get; set; }
    }
}
