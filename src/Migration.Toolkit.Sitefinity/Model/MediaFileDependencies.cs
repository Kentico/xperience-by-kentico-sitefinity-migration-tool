using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Sitefinity.Core.Models;

namespace Migration.Toolkit.Sitefinity.Model
{
    internal class MediaFileDependencies : IImportDependencies
    {
        public required IEnumerable<MediaLibraryModel> MediaLibraries { get; set; }
        public required IEnumerable<UserInfoModel> Users { get; set; }
    }
}
