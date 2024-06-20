using Microsoft.EntityFrameworkCore;
namespace Migration.Tookit.Data.Core.EF
{
    public partial class SitefinityContext : DbContext
    {
        public SitefinityContext() { }
        public SitefinityContext(DbContextOptions<SitefinityContext> options) : base(options)
        {
        }


    }
}
