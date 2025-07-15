using CMS.Core;
using CMS.Membership;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Sitefinity.Configuration;
using Migration.Toolkit.Sitefinity.Core.Helpers;

namespace Migration.Toolkit.Sitefinity.Helpers;

/// <summary>
/// Helper for user-related operations during migration
/// </summary>
internal class UserHelper(ILogger<UserHelper> logger, SitefinityImportConfiguration configuration) : IUserHelper
{
    private UserInfoModel? administratorUser;

    public UserInfoModel? GetAdministratorUser()
    {
        if (administratorUser != null)
        {
            return administratorUser;
        }

        // Get the administrator user from Kentico using the configured name
        var adminUser = Service.Resolve<IUserInfoProvider>().Get(configuration.KenticoAdministratorUserName);

        administratorUser = new UserInfoModel
        {
            UserGUID = adminUser.UserGUID,
            UserName = adminUser.UserName,
            Email = adminUser.Email,
            FirstName = adminUser.FirstName,
            LastName = adminUser.LastName,
            UserEnabled = adminUser.UserEnabled,
            UserCreated = adminUser.UserCreated,
            UserSecurityStamp = adminUser.UserSecurityStamp
        };

        return administratorUser;
    }

    public UserInfoModel? GetUserWithFallback(Guid userGuid, IDictionary<Guid, UserInfoModel> users)
    {
        // Try to get the user from the imported users dictionary
        if (users.TryGetValue(userGuid, out var user))
        {
            return user;
        }

        var adminUser = GetAdministratorUser();
        logger.LogWarning("User with GUID {UserGuid} not found. Falling back to user '{AdministratorName}'.", userGuid, adminUser?.UserName);

        return adminUser;
    }
}
