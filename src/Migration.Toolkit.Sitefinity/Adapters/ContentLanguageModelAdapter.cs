using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;

namespace Migration.Toolkit.Sitefinity.Adapters;
internal class ContentLanguageModelAdapter(ILogger<ContentLanguageModelAdapter> logger) : UmtAdapterBase<SystemCulture, ContentLanguageModel>(logger)
{
    protected override ContentLanguageModel? AdaptInternal(SystemCulture source) => new()
    {
        ContentLanguageDisplayName = source.DisplayName,
        ContentLanguageIsDefault = source.IsDefault,
        ContentLanguageCultureFormat = source.Culture,
        ContentLanguageName = source.Key,
        ContentLanguageGUID = source.Id
    };
}
