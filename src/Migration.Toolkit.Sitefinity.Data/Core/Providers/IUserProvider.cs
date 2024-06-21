using Migration.Tookit.Data.Models;

namespace Migration.Tookit.Data.Core.Providers;
/// <summary>
/// Provider for getting users from Sitefinity using Entity Framework
/// </summary>
public interface IUserProvider
{
    /// <summary>
    /// Gets all users from Sitefinity using Entity Framework
    /// </summary>
    /// <returns>List of users</returns>
    IEnumerable<User> GetUsers();
}
