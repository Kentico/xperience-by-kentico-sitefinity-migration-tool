using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

namespace Migration.Toolkit.Sitefinity.Core.Services;
/// <summary>
/// Service for importing media libraries from Sitefinity to XbyK site
/// </summary>
internal interface IMediaLibraryImportService : IDataImportService<MediaLibraryModel>
{
    ImportStateObserver StartImport(ImportStateObserver observer, out IEnumerable<MediaLibraryModel> libraries);
}
