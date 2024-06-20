using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Sitefinity.Core.Services;

namespace Migration.Toolkit.Sitefinity.Services;
internal class SitefinityImportService(IUserImportService userImportService,
                                        IDataClassImportService contentTypeImportService,
                                        IMediaImportService mediaImportService) : ISitefinityImportService
{
    public ImportStateObserver StartImportUsers(ImportStateObserver observer) => userImportService.StartImport(observer).Observer;
    public ImportStateObserver StartImportContentTypes(ImportStateObserver observer) => contentTypeImportService.StartImport(observer).Observer;
    public ImportStateObserver StartImportMedia(ImportStateObserver observer) => mediaImportService.StartImport(observer).Observer;
}
