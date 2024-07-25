using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Sitefinity.Core.Services;

namespace Migration.Toolkit.Sitefinity.Services;
internal class SitefinityImportService(IUserImportService userImportService,
                                        IDataClassImportService contentTypeImportService) : ISitefinityImportService
{
    public ImportStateObserver StartImportUsers(ImportStateObserver observer) => userImportService.StartImport(observer);


    public ImportStateObserver StartImportContentTypes(ImportStateObserver observer) => contentTypeImportService.StartImport(observer);
}
