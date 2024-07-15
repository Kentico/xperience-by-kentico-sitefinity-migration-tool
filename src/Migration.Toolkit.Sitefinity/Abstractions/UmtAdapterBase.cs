using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core.Adapters;
using Migration.Toolkit.Sitefinity.Core.Models;

namespace Migration.Toolkit.Sitefinity.Abstractions;
/// <summary>
/// Base class for UMT adapters. Provides logging and default checks.
/// </summary>
/// <typeparam name="TSourceModel">ISitefinityModel used in providers</typeparam>
/// <typeparam name="TTargetModel">IUmtModel used in Universal Migration Toolkit</typeparam>
/// <param name="logger">Logger</param>
internal abstract class UmtAdapterBase<TSourceModel, TTargetModel>(ILogger logger) : IUmtAdapter<TSourceModel, TTargetModel> where TSourceModel : ISitefinityModel where TTargetModel : IUmtModel
{
    public IEnumerable<TTargetModel> Adapt(IEnumerable<TSourceModel> source)
    {
        foreach (var model in source)
        {
            if (model.Equals(default(TSourceModel)))
            {
                logger.LogWarning("Source entity is null. Returning default.");
                continue;
            }

            if (model.Id == Guid.Empty)
            {
                logger.LogWarning("Source entity has an empty Id. Returning default.");
                continue;
            }

            var adaptedModel = AdaptInternal(model);

            if (Equals(adaptedModel, default(TTargetModel)) || adaptedModel == null)
            {
                logger.LogWarning("Adapted model is null. Returning default.");
                continue;
            }

            yield return adaptedModel;
        }
    }

    protected abstract TTargetModel? AdaptInternal(TSourceModel source);
}

internal abstract class UmtAdapterBase<TSourceModel, TDependenciesModel, TTargetModel>(ILogger logger) : IUmtAdapter<TSourceModel, TDependenciesModel, TTargetModel> where TSourceModel : ISitefinityModel
                                                                                                                                                where TTargetModel : IUmtModel
                                                                                                                                                where TDependenciesModel : IImportDependencies
{
    public IEnumerable<TTargetModel> Adapt(IEnumerable<TSourceModel> source, TDependenciesModel dependenciesModel)
    {
        foreach (var model in source)
        {
            if (model.Equals(default(TSourceModel)))
            {
                logger.LogWarning("Source entity is null. Returning default.");
                continue;
            }

            if (model.Id == Guid.Empty)
            {
                logger.LogWarning("Source entity has an empty Id. Returning default.");
                continue;
            }

            var adaptedModel = AdaptInternal(model, dependenciesModel);

            if (Equals(adaptedModel, default(TTargetModel)) || adaptedModel == null)
            {
                logger.LogWarning("Adapted model is null. Returning default.");
                continue;
            }

            yield return adaptedModel;
        }
    }

    protected abstract TTargetModel? AdaptInternal(TSourceModel source, TDependenciesModel dependenciesModel);
}
