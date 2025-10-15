using CMS.Helpers;

using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core.Adapters;
using Migration.Toolkit.Sitefinity.Core.Services;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Services;

internal class DataClassImportService(IImportService kenticoImportService,
                                        IChannelImportService channelImportService,
                                        ITypeProvider typeProvider,
                                        IUmtAdapterWithDependencies<SitefinityType, DataClassDependencies> adapter) : IDataClassImportService
{
    public IEnumerable<IUmtModel> Get(DataClassDependencies dependenciesModel)
    {
        var dataClassModels = new List<IUmtModel>();

        var types = typeProvider.GetDynamicModuleTypes();
        dataClassModels.AddRange(adapter.Adapt(types, dependenciesModel));

        var staticTypes = typeProvider.GetSitefinityTypes();
        dataClassModels.AddRange(adapter.Adapt(staticTypes, dependenciesModel));

        var mediaTypes = typeProvider.GetMediaContentTypes();
        dataClassModels.AddRange(adapter.Adapt(mediaTypes, dependenciesModel));

        return dataClassModels;
    }

    public SitefinityImportResult StartImport(ImportStateObserver observer)
    {
        var channels = channelImportService.StartImport(observer);

        observer.ImportCompletedTask.Wait();

        var dependencies = new DataClassDependencies
        {
            Channels = channels.ImportedModels.Values.OfType<ChannelModel>().ToDictionary(x => x.ChannelGUID)
        };

        return Import(observer, dependencies);
    }

    public SitefinityImportResult StartImportWithDependencies(ImportStateObserver observer, DataClassDependencies dependenciesModel) => Import(observer, dependenciesModel);

    private SitefinityImportResult Import(ImportStateObserver observer, DataClassDependencies dependencies)
    {
        var dataClasses = Get(dependencies);

        var importedModels = new Dictionary<Guid, IUmtModel>();

        foreach (var dataClass in dataClasses.OfType<DataClassModel>())
        {
            var guid = ValidationHelper.GetGuid(dataClass.ClassGUID, Guid.Empty);

            if (guid.Equals(Guid.Empty))
            {
                continue;
            }

            importedModels.Add(guid, dataClass);
        }

        return new SitefinityImportResult
        {
            ImportedModels = importedModels,
            Observer = kenticoImportService.StartImport(dataClasses, observer)
        };
    }
}
