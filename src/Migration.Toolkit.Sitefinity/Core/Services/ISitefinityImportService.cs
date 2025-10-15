using Kentico.Xperience.UMT.Services;

namespace Migration.Toolkit.Sitefinity.Core.Services;
/// <summary>
/// Main service for importing objects and content from Sitefintiy to XbyK site. Handles all import operations.
/// </summary>
public interface ISitefinityImportService
{
    /// <summary>
    /// Starts import of users from Sitefinity to XbyK site.
    /// </summary>
    /// <param name="observer">Observer used to log any errors or warnings during the import</param>
    /// <returns>Task, that will return object that represents import state</returns>
    public ImportStateObserver StartImportUsers(ImportStateObserver observer);


    /// <summary>
    /// Starts import of content types from Sitefinity to XbyK site.
    /// </summary>
    /// <param name="observer">Observer used to log any errors or warnings during the import</param>
    /// <returns>Task, that will return object that represents import state</returns>
    public ImportStateObserver StartImportContentTypes(ImportStateObserver observer);


    /// <summary>
    /// Starts import of media from Sitefinity to XbyK site. Imports users and media libraries automatically to be used in media files.
    /// </summary>
    /// <param name="observer">Observer used to log any errors or warnings during the import</param>
    /// <returns>Task, that will return object that represents import state</returns>
    public ImportStateObserver StartImportMedia(ImportStateObserver observer);

    /// <summary>
    /// Starts import of web pages and content items from Sitefinity to XbyK site. Imports users, media files and content types automatically to be used in web pages and content items.
    /// </summary>
    /// <param name="observer">Observer used to log any errors or warnings during the import</param>
    /// <returns>Task, that will return object that represents import state</returns>
    public ImportStateObserver StartImportContent(ImportStateObserver observer);

    /// <summary>
    /// Starts import of sites in Sitefinity into website channels in XbyK site.
    /// </summary>
    /// <param name="observer">Observer used to log any errors or warnings during the import</param>
    /// <returns>Task, that will return object that represents import state</returns>
    public ImportStateObserver StartImportChannels(ImportStateObserver observer);
}
