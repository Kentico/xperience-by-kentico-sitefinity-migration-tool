using Kentico.Xperience.UMT.Model;

using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Toolkit.Sitefinity.Core.Helpers;
public interface IContentHelper
{
    public IEnumerable<ContentItemLanguageData> GetLanguageData(IEnumerable<string?> languageNames, string title, DataClassModel dataClassModel, UserInfoModel? user, SdkItem sdkItem);
}
