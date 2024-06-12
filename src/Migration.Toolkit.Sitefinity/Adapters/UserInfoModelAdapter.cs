using CMS.Helpers;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Tookit.Data.Models;
using Migration.Tookit.Sitefinity.Abstractions;

namespace Migration.Tookit.Sitefinity.Mappers
{
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
}
