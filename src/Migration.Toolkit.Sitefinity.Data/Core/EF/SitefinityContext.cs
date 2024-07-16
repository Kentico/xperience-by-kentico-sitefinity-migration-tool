using Microsoft.EntityFrameworkCore;

using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Data.Core.EF;
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
    public virtual DbSet<SitefinityMediaContent> MediaContent { get; set; }

}
