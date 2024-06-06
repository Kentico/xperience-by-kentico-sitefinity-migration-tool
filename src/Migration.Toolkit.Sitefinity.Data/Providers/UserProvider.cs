using Microsoft.EntityFrameworkCore;

using Migration.Tookit.Data.Core.EF;
using Migration.Tookit.Data.Core.Providers;
using Migration.Tookit.Data.Models;

namespace Migration.Tookit.Data.Providers;
internal class UserProvider(IDbContextFactory<SitefinityContext> sitefinityContext) : IUserProvider
{
    public IEnumerable<User> GetUsers()
    {
        using var context = sitefinityContext.CreateDbContext();
        return context.Users.ToList();
    }
}
