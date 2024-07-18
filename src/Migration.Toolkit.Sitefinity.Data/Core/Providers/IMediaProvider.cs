using Migration.Tookit.Data.Models;
using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Data.Core.Providers;
/// <summary>
/// Provider for getting libraries and media from Sitefinity using Rest Sdk.
/// </summary>
public interface IMediaProvider
{
    /// <summary>
    /// Get Image libraries from Sitefinity.
    /// </summary>
    /// <returns>List of libraries</returns>
    public IEnumerable<Library> GetImageLibraries();


    /// <summary>
    /// Get Document libraries from Sitefinity.
    /// </summary>
    /// <returns>List of libraries</returns>
    public IEnumerable<Library> GetDocumentLibraries();


    /// <summary>
    /// Get Video libraries from Sitefinity.
    /// </summary>
    /// <returns>List of libraries</returns>
    public IEnumerable<Library> GetVideoLibraries();


    /// <summary>
    /// Get Documents from Sitefinity.
    /// </summary>
    /// <returns>List of documents</returns>
    public IEnumerable<Media> GetDocuments();


    /// <summary>
    /// Get Images from Sitefinity.
    /// </summary>
    /// <returns>List of images</returns>
    public IEnumerable<Media> GetImages();


    /// <summary>
    /// Get Videos from Sitefinity.
    /// </summary>
    /// <returns>List of videos</returns>
    public IEnumerable<Media> GetVideos();
}
