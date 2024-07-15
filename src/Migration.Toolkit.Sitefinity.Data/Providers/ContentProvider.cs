using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Data.Abstractions;

using Progress.Sitefinity.RestSdk;

namespace Migration.Toolkit.Data.Providers;
internal class ContentProvider(IRestClient restClient) : RestSdkBase(restClient), IContentProvider
{
    public IEnumerable<ContentItem> GetContentItems(IEnumerable<SitefinityTypeDefinition> typeDefinitions)
    {
        var contentItems = new List<ContentItem>();

        foreach (var typeDefinition in typeDefinitions)
        {
            var getAllArgs = new GetAllArgs
            {
                Type = typeDefinition.SitefinityTypeName,
                Fields = ["*"],
            };

            var items = GetUsingBatches<ContentItem>(getAllArgs);

            foreach (var item in items)
            {
                item.DataClassGuid = typeDefinition.DataClassGuid;
            }

            contentItems.AddRange(items);
        }

        return contentItems;
    }

    public IEnumerable<Page> GetPages()
    {
        var getAllArgs = new GetAllArgs
        {
            Type = RestClientContentTypes.Pages
        };

        var pages = GetUsingBatches<Page>(getAllArgs);

        return pages;
    }
}
