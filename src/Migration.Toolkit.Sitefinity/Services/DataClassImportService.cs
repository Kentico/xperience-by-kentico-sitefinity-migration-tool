using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Tookit.Data.Core.Providers;
using Migration.Tookit.Data.Models;
using Migration.Tookit.Sitefinity.Abstractions;
using Migration.Tookit.Sitefinity.Core.Services;

namespace Migration.Tookit.Sitefinity.Services;
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

    public ImportStateObserver StartImport(ImportStateObserver observer)
    {
        var types = Get();
        return kenticoImportService.StartImport(types, observer);
    }
}
