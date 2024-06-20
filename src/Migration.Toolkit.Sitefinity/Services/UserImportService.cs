using Kentico.Xperience.UMT.Model;
using Kentico.Xperience.UMT.Services;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core.Adapters;
using Migration.Toolkit.Sitefinity.Core.Services;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Services;
internal class UserImportService(IImportService kenticoImportService,
                                    IUserProvider userProvider,
                                    IUmtAdapter<User, UserInfoModel> mapper) : IUserImportService
{
    public IEnumerable<UserInfoModel> Get()
    {
        var users = userProvider.GetUsers();

        return mapper.Adapt(users);
    }

    public SitefinityImportResult<UserInfoModel> StartImport(ImportStateObserver observer)
    {
        var importedModels = Get();

        return new SitefinityImportResult<UserInfoModel>
        {
            ImportedModels = importedModels.ToDictionary(x => x.UserGUID),
            Observer = kenticoImportService.StartImport(importedModels, observer)
        };
    }
}
