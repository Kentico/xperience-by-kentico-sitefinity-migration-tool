using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Sitefinity.Core.Models;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Core.Services;
/// <summary>
/// Service for all import services.
/// </summary>
internal interface IDataImportService
{
    /// <summary>
    /// Gets items of IUmtModel
    /// </summary>
    /// <returns>List of IUmtModel</returns>
    IEnumerable<IUmtModel> Get();


    /// <summary>
    /// Starts importing of IUmtModel items
    /// </summary>
    /// <param name="observer">Observer used in UMT import service</param>
    /// <returns>Result that includes observer and items that were imported</returns>
    public SitefinityImportResult StartImport(ImportStateObserver observer);
}

/// <summary>
/// Service for all import services
/// </summary>
/// <typeparam name="T">Model used by the Universal Migration Toolkit</typeparam>
internal interface IDataImportService<T> where T : class, IUmtModel
{
    /// <summary>
    /// Gets items of type <typeparamref name="T"/>.
    /// </summary>
    /// <returns>List of <typeparamref name="T"/></returns>
    IEnumerable<T> Get();


    /// <summary>
    /// Starts importing of items of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="observer">Observer used in UMT import service</param>
    /// <returns>Result that includes observer and items that were imported</returns>
    public SitefinityImportResult<T> StartImport(ImportStateObserver observer);
}

/// <summary>
/// Service for all import services.
/// </summary>
/// <typeparam name="TDependencies">Dependencies model used in services or adapters that require other objects to be imported</typeparam>
internal interface IDataImportServiceWithDependencies<in TDependencies> where TDependencies : class, IImportDependencies
{
    /// <summary>
    /// Gets items of type IUmtModel.
    /// </summary>
    /// <param name="dependenciesModel">Dependency objects used in adapter</param>
    /// <returns>List of IUmtModel</returns>
    IEnumerable<IUmtModel> Get(TDependencies dependenciesModel);


    /// <summary>
    /// Starts importing of items of type IUmtModel.
    /// </summary>
    /// <param name="observer">Observer used in UMT import service</param>
    /// <returns>Result that includes observer and items that were imported</returns>
    public SitefinityImportResult StartImport(ImportStateObserver observer);


    /// <summary>
    /// Starts importing of items of type IUmtModel using dependencies.
    /// </summary>
    /// <param name="observer">Observer used in UMT import service</param>
    /// <param name="dependenciesModel">Dependency objects used in import</param>
    /// <returns>Result that includes observer and items that were imported</returns>
    public SitefinityImportResult StartImportWithDependencies(ImportStateObserver observer, TDependencies dependenciesModel);
}

/// <summary>
/// Service for all import services
/// </summary>
/// <typeparam name="TDependencies">Dependencies model used in services or adapters that require other objects to be imported</typeparam>
/// <typeparam name="T">Model used by the Universal Migration Toolkit</typeparam>
internal interface IDataImportServiceWithDependencies<in TDependencies, T> where TDependencies : class, IImportDependencies where T : class, IUmtModel
{
    /// <summary>
    /// Gets items of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="dependenciesModel">Dependency objects used in adapter</param>
    /// <returns>List of <typeparamref name="T"/></returns>
    IEnumerable<T> Get(TDependencies dependenciesModel);


    /// <summary>
    /// Starts importing of items of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="observer">Observer used in UMT import service</param>
    /// <returns>Result that includes observer and items that were imported</returns>
    public SitefinityImportResult<T> StartImport(ImportStateObserver observer);


    /// <summary>
    /// Starts importing of items of type <typeparamref name="T"/> using dependencies.
    /// </summary>
    /// <param name="observer">Observer used in UMT import service</param>
    /// <param name="dependenciesModel">Dependency objects used in import</param>
    /// <returns>Result that includes observer and items that were imported</returns>
    public SitefinityImportResult<T> StartImportWithDependencies(ImportStateObserver observer, TDependencies dependenciesModel);
}
