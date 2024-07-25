using Kentico.Xperience.UMT.Model;

namespace Migration.Toolkit.Sitefinity.Core.Services;
/// <summary>
/// Service for importing users from Sitefinity to XbyK site.
/// </summary>
internal interface IUserImportService : IDataImportService<UserInfoModel>
{
}
