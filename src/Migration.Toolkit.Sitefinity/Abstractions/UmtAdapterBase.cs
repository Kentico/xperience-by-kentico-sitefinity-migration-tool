using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Tookit.Data.Models;

namespace Migration.Tookit.Sitefinity.Abstractions;
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
