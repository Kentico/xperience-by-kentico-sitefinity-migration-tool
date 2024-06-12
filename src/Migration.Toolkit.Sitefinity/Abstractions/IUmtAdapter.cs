using Kentico.Xperience.UMT.Model;

using Migration.Tookit.Data.Models;

namespace Migration.Tookit.Sitefinity.Abstractions;
public interface IUmtAdapter<in TSourceModel, out TTargetModel> where TSourceModel : ISitefinityModel where TTargetModel : IUmtModel
{
    IEnumerable<TTargetModel> Adapt(IEnumerable<TSourceModel> source);
}
