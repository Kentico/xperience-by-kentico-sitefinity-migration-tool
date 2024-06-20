using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Core.Services;
/// <summary>
/// Service for importing media files from Sitefinity to XbyK site
/// </summary>
internal interface IMediaImportService : IDataImportService<MediaFileDependencies, MediaFileModel>
{
}
