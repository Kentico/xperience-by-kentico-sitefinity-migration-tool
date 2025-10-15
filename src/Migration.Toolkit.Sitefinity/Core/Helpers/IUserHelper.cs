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
}
