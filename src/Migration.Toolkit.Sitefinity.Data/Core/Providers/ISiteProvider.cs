using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Data.Core.Providers;
/// <summary>
/// Provider for getting sites from Sitefinity.
/// </summary>
public interface ISiteProvider
{
    /// <summary>
    /// Gets Sites from Sitefinity using site definition xml file in Deployment export.
    /// </summary>
    /// <returns>Sites from Sitefinity.</returns>
    public IEnumerable<Site> GetSites();
}
