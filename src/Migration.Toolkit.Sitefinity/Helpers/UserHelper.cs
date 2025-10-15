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

        if (adminUser == null)
        {
            logger.LogError("Administrator user '{AdministratorName}' not found in Kentico database. Please ensure the user exists.", configuration.KenticoAdministratorUserName);
            return null;
        }

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
}
