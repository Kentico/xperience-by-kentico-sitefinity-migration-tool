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
/// <param name="logger">Logger</param>
internal abstract class UmtAdapterBase<TSourceModel>(ILogger logger) : IUmtAdapter<TSourceModel> where TSourceModel : ISitefinityModel
{
    public IEnumerable<IUmtModel> Adapt(IEnumerable<TSourceModel> source)
    {
        var adaptedModelsList = new List<IUmtModel>();

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

            var adaptedModels = AdaptInternal(model);

            if (adaptedModels == null || !adaptedModels.Any())
            {
                logger.LogWarning("Adapted models are empty. Skipping.");
                continue;
            }

            adaptedModelsList.AddRange(adaptedModels);
        }

        return adaptedModelsList;
    }

    protected abstract IEnumerable<IUmtModel>? AdaptInternal(TSourceModel source);
}

internal abstract class UmtAdapterBaseWithDependencies<TSourceModel, TDependenciesModel>(ILogger logger) : IUmtAdapterWithDependencies<TSourceModel, TDependenciesModel> where TSourceModel : ISitefinityModel where TDependenciesModel : IImportDependencies
{
    public IEnumerable<IUmtModel> Adapt(IEnumerable<TSourceModel> source, TDependenciesModel dependenciesModel)
    {
        var adaptedModelsList = new List<IUmtModel>();

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

            var adaptedModels = AdaptInternal(model, dependenciesModel);

            if (adaptedModels == null || !adaptedModels.Any())
            {
                logger.LogWarning("Adapted models are empty. Skipping.");
                continue;
            }

            adaptedModelsList.AddRange(adaptedModels);
        }

        return adaptedModelsList;
    }

    protected abstract IEnumerable<IUmtModel>? AdaptInternal(TSourceModel source, TDependenciesModel dependenciesModel);
}

/// <summary>
/// Base class for UMT adapters. Provides logging and default checks.
/// </summary>
/// <typeparam name="TSourceModel">ISitefinityModel used in providers</typeparam>
/// <typeparam name="TTargetModel">IUmtModel used in Universal Migration Toolkit</typeparam>
/// <param name="logger">Logger</param>
internal abstract class UmtAdapterBase<TSourceModel, TTargetModel>(ILogger logger) : IUmtAdapter<TSourceModel, TTargetModel> where TSourceModel : ISitefinityModel where TTargetModel : class, IUmtModel
{
    public IEnumerable<TTargetModel> Adapt(IEnumerable<TSourceModel> source)
    {
        var adaptedModelsList = new List<TTargetModel>();

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

            if (Equals(adaptedModel, default(TTargetModel)))
            {
                logger.LogWarning("Adapted model is null. Returning default.");
                continue;
            }

            adaptedModelsList.Add(adaptedModel);
        }

        return adaptedModelsList;
    }

    protected abstract TTargetModel? AdaptInternal(TSourceModel source);
}

/// <summary>
/// Base class for UMT adapters. Provides logging and default checks.
/// </summary>
/// <typeparam name="TSourceModel">ISitefinityModel used in providers</typeparam>
/// <typeparam name="TDependenciesModel">IImportDependencies model used for dependencies used in adapter</typeparam>
/// <typeparam name="TTargetModel">IUmtModel used in Universal Migration Toolkit</typeparam>
/// <param name="logger">Logger</param>
internal abstract class UmtAdapterBaseWithDependencies<TSourceModel, TDependenciesModel, TTargetModel>(ILogger logger) : IUmtAdapterWithDependencies<TSourceModel, TDependenciesModel, TTargetModel> where TSourceModel : ISitefinityModel
                                                                                                                                                where TTargetModel : class, IUmtModel
                                                                                                                                                where TDependenciesModel : IImportDependencies
{
    public IEnumerable<TTargetModel> Adapt(IEnumerable<TSourceModel> source, TDependenciesModel dependenciesModel)
    {
        var adaptedModelsList = new List<TTargetModel>();

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

            if (Equals(adaptedModel, default(TTargetModel)))
            {
                logger.LogWarning("Adapted model is null. Returning default.");
                continue;
            }

            adaptedModelsList.Add(adaptedModel);
        }

        return adaptedModelsList;
    }

    protected abstract TTargetModel? AdaptInternal(TSourceModel source, TDependenciesModel dependenciesModel);
}
