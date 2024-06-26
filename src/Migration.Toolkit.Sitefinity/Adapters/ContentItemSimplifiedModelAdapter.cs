using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Tookit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Adapters;
internal class ContentItemSimplifiedModelAdapter(ILogger<ContentItemSimplifiedModelAdapter> logger) : UmtAdapterBase<ContentItem, ContentDependencies, ContentItemSimplifiedModel>(logger)
{
    protected override ContentItemSimplifiedModel? AdaptInternal(ContentItem source, ContentDependencies dependenciesModel)
    {
        if (!dependenciesModel.DataClasses.TryGetValue(source.DataClassGuid, out var dataClassModel))
        {
            logger.LogWarning($"Data class with ClassGuid of {source.DataClassGuid} not found. Skipping content item {source.ItemDefaultUrl}.");
            return default;
        }

        //var languageData = new List<ContentItemLanguageData>();

        //languageData.Add(new ContentItemLanguageData { a})


        return new ContentItemSimplifiedModel
        {
            ContentItemGUID = source.Id,
            ContentTypeName = dataClassModel.ClassName,
            Name = source.UrlName,
        };
    }
}
