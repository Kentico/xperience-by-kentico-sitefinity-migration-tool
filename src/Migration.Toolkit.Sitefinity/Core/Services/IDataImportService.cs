using Kentico.Xperience.UMT.Model;

namespace Migration.Toolkit.Sitefinity.Core.Services;
/// <summary>
/// Base class for all import services
/// </summary>
/// <typeparam name="T">Model used by the Universal Migration Toolkit</typeparam>
public interface IDataImportService<out T> where T : class, IUmtModel
{
    /// <summary>
    /// Gets items of type <typeparamref name="T"/>
    /// </summary>
    /// <returns>List of <typeparamref name="T"/></returns>
    IEnumerable<T> Get();
}
