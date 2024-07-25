using Kentico.Xperience.UMT.Services;

namespace Migration.Toolkit.Sitefinity.Core.Services;
/// <summary>
/// Main service for importing objects and content from Sitefintiy to XbyK site. Handles all import operations.
/// </summary>
public interface ISitefinityImportService
{
    /// <summary>
    /// Starts import of users from Sitefinity to XbyK site
    /// </summary>
    /// <param name="observer">Observer used to log any errors or warnings during the import</param>
    /// <returns>Task, that will return object that represents import state</returns>
    ImportStateObserver StartImportUsers(ImportStateObserver observer);
    /// <summary>
    /// Starts import of content types from Sitefinity to XbyK site
    /// </summary>
    /// <param name="observer">Observer used to log any errors or warnings during the import</param>
    /// <returns>Task, that will return object that represents import state</returns>
    ImportStateObserver StartImportContentTypes(ImportStateObserver observer);
}
