using Kentico.Xperience.UMT.Model;

namespace Migration.Toolkit.Sitefinity.Core.Helpers;

/// <summary>
/// Helper for user-related operations during migration
/// </summary>
internal interface IUserHelper
{
    /// <summary>
    /// Gets the administrator user to use as fallback when Sitefinity user is not found
    /// </summary>
    /// <returns>Administrator user or null if not found</returns>
    public UserInfoModel? GetAdministratorUser();

    /// <summary>
    /// Gets a user by GUID with fallback to administrator user
    /// </summary>
    /// <param name="userGuid">User GUID to lookup</param>
    /// <param name="users">Dictionary of imported users</param>
    /// <returns>User if found, otherwise administrator user</returns>
    public UserInfoModel? GetUserWithFallback(Guid userGuid, IDictionary<Guid, UserInfoModel> users);
}
