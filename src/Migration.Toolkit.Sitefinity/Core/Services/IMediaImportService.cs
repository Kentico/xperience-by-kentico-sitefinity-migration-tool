using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Core.Services;
internal interface IMediaImportService : IDataImportService<MediaFileDependencies, MediaFileModel>
{
}
