using Kentico.Xperience.UMT.Model;

namespace System.Linq;
internal static class IEnumerableExtensions
{
    /// <summary>
    /// Creates dictionary using key selector. Removes any items with a null or empty guid.
    /// </summary>
    /// <typeparam name="TUmtModel">IUmtModel</typeparam>
    /// <param name="models">List of IUmtModel models</param>
    /// <param name="keySelector">Selector of nullable Guid property</param>
    /// <returns>Dictionary of IUmtModels with a non-null Guid key</returns>
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
