using Kentico.Xperience.UMT.Model;

namespace System.Linq;
internal static class IEnumerableExtensions
{
    public static Dictionary<Guid, TUmtModel> ToDictionary<TUmtModel>(this IEnumerable<TUmtModel> models, Func<TUmtModel, Guid?> keySelector) where TUmtModel : class, IUmtModel
    {
        var dictionary = new Dictionary<Guid, TUmtModel>();

        foreach (var model in models)
        {
            var key = keySelector(model) ?? Guid.Empty;

            if (!key.Equals(Guid.Empty))
            {
                dictionary.Add(key, model);
            }
        }

        return dictionary;
    }
}
