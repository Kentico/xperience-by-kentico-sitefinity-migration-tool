using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

namespace Migration.Toolkit.Sitefinity.Core.Services;
/// <summary>
/// Service for importing users from Sitefinity to XbyK site
/// </summary>
internal interface IUserImportService : IDataImportService<UserInfoModel>
{
    ImportStateObserver StartImport(ImportStateObserver observer);
    ImportStateObserver StartImport(ImportStateObserver observer, out IEnumerable<UserInfoModel> users);
}
