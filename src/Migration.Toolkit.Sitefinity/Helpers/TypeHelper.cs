using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Configuration;
using Migration.Toolkit.Sitefinity.Core.Helpers;

namespace Migration.Toolkit.Sitefinity.Helpers;

internal class TypeHelper(ITypeProvider typeProvider, SitefinityImportConfiguration importConfiguration) : ITypeHelper
{
    private IEnumerable<SitefinityType>? types;

    public IEnumerable<SitefinityType> GetWebsiteTypes()
    {
        var pageConfig = importConfiguration.PageContentTypes;

        if (pageConfig == null)
        {
            return [];
        }

        var websiteTypes = new List<SitefinityType>();

        types ??= typeProvider.GetAllTypes();

        websiteTypes.AddRange(types.Where(t => pageConfig.Any(x => x.TypeName.Equals(t.Name))));

        var childTypes = websiteTypes.SelectMany(t => FindChildWebsiteTypes(t, types)).ToList();

        websiteTypes.AddRange(childTypes);

        return websiteTypes.Distinct();
    }

    private static List<SitefinityType> FindChildWebsiteTypes(SitefinityType parentType, IEnumerable<SitefinityType> types)
    {
        var sitefinityTypes = new List<SitefinityType>();

        var childTypes = types.Where(t => t.ParentModuleTypeId == parentType.Id);

        foreach (var childType in childTypes)
        {
            sitefinityTypes.Add(childType);

            while (FindChildWebsiteTypes(childType, types).Count != 0)
            {
                sitefinityTypes.AddRange(FindChildWebsiteTypes(childType, types));
            }
        }

        return sitefinityTypes;
    }
}
