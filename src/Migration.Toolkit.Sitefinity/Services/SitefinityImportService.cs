using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Sitefinity.Core.Services;

namespace Migration.Toolkit.Sitefinity.Services;
internal class SitefinityImportService(IUserImportService userImportService,
                                        IDataClassImportService contentTypeImportService,
                                        IMediaLibraryImportService mediaLibraryImportService) : ISitefinityImportService
{
    public ImportStateObserver StartImportUsers(ImportStateObserver observer) => userImportService.StartImport(observer);
    public ImportStateObserver StartImportContentTypes(ImportStateObserver observer) => contentTypeImportService.StartImport(observer);
    public ImportStateObserver StartImportMedia(ImportStateObserver observer) => mediaLibraryImportService.StartImport(observer, out _);
}
