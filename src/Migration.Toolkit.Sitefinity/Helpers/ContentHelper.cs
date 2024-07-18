using CMS.ContentEngine;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Sitefinity.Core.Factories;
using Migration.Toolkit.Sitefinity.Core.Helpers;

using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Toolkit.Sitefinity.Helpers;
internal class ContentHelper(ILogger<ContentHelper> logger, ITypeProvider typeProvider, IFieldTypeFactory fieldTypeFactory) : IContentHelper
{
    public IEnumerable<ContentItemLanguageData> GetLanguageData(IEnumerable<string?> languageNames, string title, DataClassModel dataClassModel, UserInfoModel? user, SdkItem sdkItem)
    {
        var types = typeProvider.GetAllTypes();

        var type = types.FirstOrDefault(x => x.Id == dataClassModel.ClassGUID);

        if (type == null || type.Fields == null)
        {
            return [];
        }

        var languageData = new List<ContentItemLanguageData>();

        foreach (string? languageName in languageNames.Take(1))
        {
            if (languageName == null)
            {
                continue;
            }
            var contentItemData = new Dictionary<string, object?>();

            foreach (var field in type.Fields)
            {
                var fieldType = fieldTypeFactory.CreateFieldType(field.WidgetTypeName);

                if (field.Name == null)
                {
                    continue;
                }

                if (Constants.ExcludedFields.Contains(field.Name))
                {
                    continue;
                }

                try
                {
                    contentItemData.Add(field.Name, fieldType.GetData(sdkItem, field.Name));
                }
                catch
                {
                    logger.LogWarning("Cannot get data for {FieldName} field", field.Name);
                }
            }

            languageData.Add(new ContentItemLanguageData
            {
                DisplayName = title,
                LanguageName = languageName,
                UserGuid = user?.UserGUID,
                ContentItemData = contentItemData,
                VersionStatus = VersionStatus.Published,
            });
        }

        return languageData;
    }
}
