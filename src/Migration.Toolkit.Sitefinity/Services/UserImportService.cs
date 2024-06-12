using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Tookit.Data.Core.Providers;
using Migration.Tookit.Data.Models;
using Migration.Tookit.Sitefinity.Abstractions;
using Migration.Tookit.Sitefinity.Core.Services;

namespace Migration.Tookit.Sitefinity.Services;
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
