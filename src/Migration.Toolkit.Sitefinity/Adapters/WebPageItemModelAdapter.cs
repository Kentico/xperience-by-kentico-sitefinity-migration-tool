using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Adapters;
internal class WebPageItemModelAdapter(ILogger<WebPageItemModelAdapter> logger) : UmtAdapterBase<Page, ContentDependencies, WebPageItemModel>(logger)
{
    protected override WebPageItemModel? AdaptInternal(Page source, ContentDependencies dependenciesModel) => new()
    {
        WebPageItemGUID = source.Id
    };
}
