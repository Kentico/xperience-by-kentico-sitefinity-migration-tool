using Kentico.Xperience.UMT.Services;

namespace Migration.Tookit.Sitefinity.Core.Services;
public interface ISitefinityImportService
{
    ImportStateObserver StartImportUsers(ImportStateObserver observer);
}
