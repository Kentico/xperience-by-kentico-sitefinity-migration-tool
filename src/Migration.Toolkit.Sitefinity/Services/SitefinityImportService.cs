using Kentico.Xperience.UMT.Services;

using Migration.Tookit.Sitefinity.Core.Services;

namespace Migration.Tookit.Sitefinity.Services;
internal class SitefinityImportService(IUserImportService userImportService,
                                        IDataClassImportService contentTypeImportService) : ISitefinityImportService
{
    public ImportStateObserver StartImportUsers(ImportStateObserver observer) => userImportService.StartImport(observer);


    public ImportStateObserver StartImportDynamicTypes(ImportStateObserver observer) => contentTypeImportService.StartImport(observer);
}
