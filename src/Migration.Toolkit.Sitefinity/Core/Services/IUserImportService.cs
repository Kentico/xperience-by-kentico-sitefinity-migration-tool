using Kentico.Xperience.UMT.Model;

namespace Migration.Tookit.Sitefinity.Core.Services;
/// <summary>
/// Service for importing users from Sitefinity to XbyK site
/// </summary>
public interface IUserImportService : IDataImportService<UserInfoModel>
{
}
