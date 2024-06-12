namespace Migration.Toolkit.Sitefinity.Core.Services;
/// <summary>
/// Base class for all import services
/// </summary>
/// <typeparam name="T">Model used by the Universal Migration Toolkit</typeparam>
public interface IDataImportService<out T> where T : class
{
    IEnumerable<T> Get();
}
