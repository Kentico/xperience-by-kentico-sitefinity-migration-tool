using Kentico.Xperience.UMT.Services;

using Migration.Tookit.Sitefinity.Core.Services;

namespace Migration.Tookit.Sitefinity.Services;
public class SitefinityImportService(IImportService kenticoImportService,
                                        IUserImportService userProvider) : ISitefinityImportService
{
    public ImportStateObserver StartImportUsers(ImportStateObserver observer)
    {
        var users = userProvider.Get();

        return kenticoImportService.StartImport(users, observer);
    }
}
