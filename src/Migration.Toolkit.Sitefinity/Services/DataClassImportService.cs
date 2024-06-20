using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core.Adapters;
using Migration.Toolkit.Sitefinity.Core.Services;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Services;
internal class DataClassImportService(IImportService kenticoImportService, ITypeProvider typeProvider, IUmtAdapter<SitefinityType, DataClassModel> mapper) : IDataClassImportService
{
    public IEnumerable<DataClassModel> Get()
    {
        var dataClassModels = new List<DataClassModel>();

        var types = typeProvider.GetDynamicModuleTypes();
        dataClassModels.AddRange(mapper.Adapt(types));

        var staticTypes = typeProvider.GetSitefinityTypes();
        dataClassModels.AddRange(mapper.Adapt(staticTypes));

        return dataClassModels;
    }

    public SitefinityImportResult<DataClassModel> StartImport(ImportStateObserver observer) => new()
    {
        ImportedModels = Get(),
        Observer = kenticoImportService.StartImport(Get(), observer)
    };
}
