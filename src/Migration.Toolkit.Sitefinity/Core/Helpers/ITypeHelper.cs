using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Sitefinity.Core.Helpers;
internal interface ITypeHelper
{
    public IEnumerable<SitefinityType> GetWebsiteTypes();
}
