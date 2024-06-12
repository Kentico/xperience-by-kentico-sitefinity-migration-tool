using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Data.Core.Providers;
public interface ITypeProvider
{
    IEnumerable<SitefinityType> GetDynamicModuleTypes();
    IEnumerable<SitefinityType> GetSitefinityTypes();
}
