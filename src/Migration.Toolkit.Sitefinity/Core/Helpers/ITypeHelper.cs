using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Sitefinity.Core.Helpers;
/// <summary>
/// Helper class for all type related operations.
/// </summary>
internal interface ITypeHelper
{
    /// <summary>
    /// Gets types that are considered as website based on the PageContentTypes setting.
    /// </summary>
    /// <returns>Website types.</returns>
    public IEnumerable<SitefinityType> GetWebsiteTypes();
}
