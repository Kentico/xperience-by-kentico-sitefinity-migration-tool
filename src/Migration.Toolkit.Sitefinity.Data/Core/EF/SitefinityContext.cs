using Microsoft.EntityFrameworkCore;

using Migration.Tookit.Data.Models;

namespace Migration.Tookit.Data.Core.EF;
/// <summary>
/// Entity Framework Context for Sitefinity database
/// </summary>
public partial class SitefinityContext : DbContext
{
    public SitefinityContext() { }
    public SitefinityContext(DbContextOptions<SitefinityContext> options) : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

}
