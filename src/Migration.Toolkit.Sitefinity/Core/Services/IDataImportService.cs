using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Sitefinity.Core.Models;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Core.Services;
/// <summary>
/// Base class for all import services
/// </summary>
/// <typeparam name="T">Model used by the Universal Migration Toolkit</typeparam>
public interface IDataImportService<T> where T : class, IUmtModel
{
    /// <summary>
    /// Gets items of type <typeparamref name="T"/>
    /// </summary>
    /// <returns>List of <typeparamref name="T"/></returns>
    IEnumerable<T> Get();
    public SitefinityImportResult<T> StartImport(ImportStateObserver observer);
}

public interface IDataImportService<in TDependencies, T> where TDependencies : class, IImportDependencies where T : class, IUmtModel
{
    /// <summary>
    /// Gets items of type <typeparamref name="T"/>
    /// </summary>
    /// <returns>List of <typeparamref name="T"/></returns>
    IEnumerable<T> Get(TDependencies dependenciesModel);
    public SitefinityImportResult<T> StartImport(ImportStateObserver observer);
}
