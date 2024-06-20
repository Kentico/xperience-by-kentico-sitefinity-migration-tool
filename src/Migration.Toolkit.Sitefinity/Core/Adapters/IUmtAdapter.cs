using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core.Models;

namespace Migration.Toolkit.Sitefinity.Core.Adapters;
/// <summary>
/// Adapter for adapting Sitefinity models to UMT models
/// </summary>
/// <typeparam name="TSourceModel">ISitefinityModel used in providers</typeparam>
/// <typeparam name="TTargetModel">IUmtModel used in Universal Migration Toolkit</typeparam>
public interface IUmtAdapter<in TSourceModel, out TTargetModel> where TSourceModel : ISitefinityModel where TTargetModel : IUmtModel
{
    IEnumerable<TTargetModel> Adapt(IEnumerable<TSourceModel> source);
}

public interface IUmtAdapter<in TSourceModel, in TDependenciesModel, out TTargetModel> where TSourceModel : ISitefinityModel where TDependenciesModel : IImportDependencies where TTargetModel : IUmtModel
{
    IEnumerable<TTargetModel> Adapt(IEnumerable<TSourceModel> source, TDependenciesModel dependenciesModel);
}
