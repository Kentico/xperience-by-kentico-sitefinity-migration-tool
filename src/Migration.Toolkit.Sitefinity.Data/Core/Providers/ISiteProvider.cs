using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Data.Core.Providers;
public interface ISiteProvider
{
    public IEnumerable<Site> GetSites();
}
