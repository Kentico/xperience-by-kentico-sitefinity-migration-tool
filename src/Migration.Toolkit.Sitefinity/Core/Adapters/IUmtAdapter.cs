using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Sitefinity.Core.Adapters;
public interface IUmtAdapter<in TSourceModel, out TTargetModel> where TSourceModel : ISitefinityModel where TTargetModel : IUmtModel
{
    IEnumerable<TTargetModel> Adapt(IEnumerable<TSourceModel> source);
}
