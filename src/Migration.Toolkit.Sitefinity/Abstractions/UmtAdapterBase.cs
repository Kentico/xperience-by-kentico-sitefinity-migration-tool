using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core.Adapters;

namespace Migration.Toolkit.Sitefinity.Abstractions;
/// <summary>
/// Base class for UMT adapters. Provides logging and default checks.
/// </summary>
/// <typeparam name="TSourceModel">ISitefinityModel used in providers</typeparam>
/// <typeparam name="TTargetModel">IUmtModel used in Universal Migration Toolkit</typeparam>
/// <param name="logger">Logger</param>
public abstract class UmtAdapterBase<TSourceModel, TTargetModel>(ILogger logger) : IUmtAdapter<TSourceModel, TTargetModel> where TSourceModel : ISitefinityModel where TTargetModel : IUmtModel
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

            yield return AdaptInternal(model);
        }
    }

    protected abstract TTargetModel AdaptInternal(TSourceModel source);
}
