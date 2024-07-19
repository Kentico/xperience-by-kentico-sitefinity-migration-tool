using CMS.ContentEngine;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;

namespace Migration.Toolkit.Sitefinity.Adapters;
internal class ContentLanguageModelAdapter(ILogger<ContentLanguageModelAdapter> logger) : UmtAdapterBase<SystemCulture, ContentLanguageModel>(logger)
{
    protected override ContentLanguageModel? AdaptInternal(SystemCulture source)
    {
        var existing = ContentLanguageInfoProvider.ProviderObject.Get()
                    .WhereEquals(nameof(ContentLanguageInfo.ContentLanguageCultureFormat), source.Culture)
                    .FirstOrDefault();

        if (existing != null)
        {
            return AdaptLanguage(existing);
        }

        Guid? fallbackLanguageGuid = null;

        if (!source.IsDefault)
        {
            var defaultCulture = ContentLanguageInfoProvider.ProviderObject.Get()
                    .WhereEquals(nameof(ContentLanguageInfo.ContentLanguageIsDefault), true)
                    .FirstOrDefault();

            fallbackLanguageGuid = defaultCulture?.ContentLanguageGUID;
        }

        return new ContentLanguageModel
        {
            ContentLanguageDisplayName = source.DisplayName,
            ContentLanguageIsDefault = source.IsDefault,
            ContentLanguageCultureFormat = source.Culture,
            ContentLanguageName = source.Key,
            ContentLanguageGUID = Guid.NewGuid(),
            ContentLanguageFallbackContentLanguageGuid = fallbackLanguageGuid,
        };
    }

    private ContentLanguageModel? AdaptLanguage(ContentLanguageInfo existing) => new()
    {
        ContentLanguageDisplayName = existing.ContentLanguageDisplayName,
        ContentLanguageIsDefault = existing.ContentLanguageIsDefault,
        ContentLanguageCultureFormat = existing.ContentLanguageCultureFormat,
        ContentLanguageName = existing.ContentLanguageName,
        ContentLanguageGUID = existing.ContentLanguageGUID,
    };
}
