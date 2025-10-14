using CMS.Helpers;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;

namespace Migration.Toolkit.Sitefinity.Adapters;

internal class UserInfoModelAdapter(ILogger<UserInfoModelAdapter> logger) : UmtAdapterBase<User, UserInfoModel>(logger)
{
    protected override UserInfoModel AdaptInternal(User source)
    {
        var random = new Random();

        return new UserInfoModel
        {
            UserGUID = ValidationHelper.GetGuid(source.Id, Guid.Empty),
            UserName = source.UserName,
            Email = source.Email,
            FirstName = source.FirstName,
            LastName = source.LastName,
            UserPassword = SecurityHelper.GetSHA2Hash("ImportTemp" + random.Next()),
            UserEnabled = true,
            UserIsPendingRegistration = false,
            UserIsExternal = false,
            UserAdministrationAccess = source.IsBackendUser
        };
    }
}
