using CMS.Helpers;

using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Tookit.Data.Core.Providers;
using Migration.Tookit.Sitefinity.Core.Services;

namespace Migration.Tookit.Sitefinity.Services;
internal class UserImportService(IImportService kenticoImportService, IUserProvider userProvider) : IUserImportService
{
    public IEnumerable<UserInfoModel> Get()
    {
        var users = userProvider.GetUsers();
        var random = new Random();

        return users.Select(user => new UserInfoModel
        {
            UserGUID = ValidationHelper.GetGuid(user.Id, Guid.Empty),
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserPassword = SecurityHelper.GetSHA2Hash("ImportTemp" + random.Next()),
            UserEnabled = true,
            UserIsPendingRegistration = false,
            UserIsExternal = false,
            UserAdministrationAccess = user.IsBackendUser
        });
    }

    public ImportStateObserver StartImport(ImportStateObserver observer)
    {
        var users = Get();

        return kenticoImportService.StartImport(users, observer);
    }
}
