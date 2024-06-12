using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Core.Services;

namespace Migration.Toolkit.Sitefinity.Services;
internal class UserImportService(IImportService kenticoImportService,
                                    IUserProvider userProvider,
                                    IUmtAdapter<User, UserInfoModel> mapper) : IUserImportService
{
    public IEnumerable<UserInfoModel> Get()
    {
        var users = userProvider.GetUsers();

        return mapper.Adapt(users);
    }

    public ImportStateObserver StartImport(ImportStateObserver observer, out IEnumerable<UserInfoModel> users)
    {
        users = Get();

        return kenticoImportService.StartImport(users, observer);
    }

    public ImportStateObserver StartImport(ImportStateObserver observer)
    {
        var users = Get();

        return kenticoImportService.StartImport(users, observer);
    }
}
