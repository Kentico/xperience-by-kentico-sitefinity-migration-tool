using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Data.Core.Providers;
/// <summary>
/// Provider for getting static and dynamic module types from Sitefinity
/// </summary>
public interface ITypeProvider
{
    /// <summary>
    /// Get all dynamic module types using the exported Deployment .sf files
    /// </summary>
    /// <returns>List of Sitefinity types</returns>
    IEnumerable<SitefinityType> GetDynamicModuleTypes();

    /// <summary>
    /// Gets static Sitefinity types defined in the StaticSitefinityTypes folder
    /// </summary>
    /// <returns>List of Sitefinity types</returns>
    IEnumerable<SitefinityType> GetSitefinityTypes();

    /// <summary>
    /// Gets both static and dynamic module types
    /// </summary>
    /// <returns>List of Sitefintiy types</returns>
    IEnumerable<SitefinityType> GetAllTypes();
}
