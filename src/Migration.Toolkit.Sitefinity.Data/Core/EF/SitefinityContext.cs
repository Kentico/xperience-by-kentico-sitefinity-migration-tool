using Microsoft.EntityFrameworkCore;

using Migration.Tookit.Data.Models;

namespace Migration.Tookit.Data.Core.EF;
public partial class SitefinityContext : DbContext
{
    public SitefinityContext() { }
    public SitefinityContext(DbContextOptions<SitefinityContext> options) : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

}
