using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core.Models;

namespace Migration.Toolkit.Sitefinity.Core.Adapters;
/// <summary>
/// Adapter for adapting Sitefinity models to UMT models
/// </summary>
/// <typeparam name="TSourceModel">ISitefinityModel used in providers</typeparam>
/// <typeparam name="TTargetModel">IUmtModel used in Universal Migration Toolkit</typeparam>
internal interface IUmtAdapter<in TSourceModel, out TTargetModel> where TSourceModel : ISitefinityModel where TTargetModel : IUmtModel
{
    /// <summary>
    /// Adapts <typeparamref name="TSourceModel"/> to <typeparamref name="TTargetModel"/>
    /// </summary>
    /// <param name="source">List of source models</param>
    /// <returns>List of adapted models</returns>
    IEnumerable<TTargetModel> Adapt(IEnumerable<TSourceModel> source);
}

/// <summary>
/// Adapter for adapting Sitefinity models to UMT models
/// </summary>
/// <typeparam name="TSourceModel">ISitefinityModel used in providers</typeparam>
/// <typeparam name="TDependenciesModel">IImportDependencies used to pass dependent objects used in adapter</typeparam>
/// <typeparam name="TTargetModel">IUmtModel used in Universal Migration Toolkit</typeparam>
internal interface IUmtAdapter<in TSourceModel, in TDependenciesModel, out TTargetModel> where TSourceModel : ISitefinityModel where TDependenciesModel : IImportDependencies where TTargetModel : IUmtModel
{
    /// <summary>
    /// Adapts <typeparamref name="TSourceModel"/> to <typeparamref name="TTargetModel"/>
    /// </summary>
    /// <param name="source">List of source models</param>
    /// <param name="dependenciesModel">Dependency objects used in adapter</param>
    /// <returns>List of adapted models</returns>
    IEnumerable<TTargetModel> Adapt(IEnumerable<TSourceModel> source, TDependenciesModel dependenciesModel);
}
