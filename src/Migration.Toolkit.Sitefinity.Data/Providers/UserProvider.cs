using Microsoft.EntityFrameworkCore;

using Migration.Toolkit.Data.Core.EF;
using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Data.Providers;
internal class UserProvider(IDbContextFactory<SitefinityContext> sitefinityContext) : IUserProvider
{
    public IEnumerable<User> GetUsers()
    {
        using var context = sitefinityContext.CreateDbContext();
        return context.Users.ToList();
    }
}
