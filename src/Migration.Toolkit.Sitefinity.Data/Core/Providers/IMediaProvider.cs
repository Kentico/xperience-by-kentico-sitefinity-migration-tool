using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Data.Core.Providers;
/// <summary>
/// Provider for getting libraries and media from Sitefinity using Rest Sdk
/// </summary>
public interface IMediaProvider
{
    /// <summary>
    /// Get Image libraries from Sitefinity
    /// </summary>
    /// <returns>List of libraries</returns>
    public IEnumerable<Library> GetImageLibraries();
    /// <summary>
    /// Get Document libraries from Sitefinity
    /// </summary>
    /// <returns>List of libraries</returns>
    public IEnumerable<Library> GetDocumentLibraries();
    /// <summary>
    /// Get Video libraries from Sitefinity
    /// </summary>
    /// <returns>List of libraries</returns>
    public IEnumerable<Library> GetVideoLibraries();
}
