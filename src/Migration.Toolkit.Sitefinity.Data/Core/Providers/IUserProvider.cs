using Migration.Tookit.Data.Models;

namespace Migration.Tookit.Data.Core.Providers;
public interface IUserProvider
{
    IEnumerable<User> GetUsers();
}
