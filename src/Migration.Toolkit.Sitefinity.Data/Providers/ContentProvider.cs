using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Data.Abstractions;

using Progress.Sitefinity.RestSdk;
using Microsoft.EntityFrameworkCore;
using Migration.Toolkit.Data.Core.EF;

namespace Migration.Toolkit.Data.Providers;
internal class ContentProvider(IRestClient restClient, IDbContextFactory<SitefinityContext> sitefinityContext) : RestSdkBase(restClient), IContentProvider
{
    private IEnumerable<SitefinityVersionChange>? versions;
    private IEnumerable<SitefinityPageNode>? pageNodes;

    public IEnumerable<ContentItem> GetContentItems(IEnumerable<SitefinityTypeDefinition> typeDefinitions, IEnumerable<string?> cultures)
    {
        using var context = sitefinityContext.CreateDbContext();
        versions ??= context.VersionChanges.OrderByDescending(x => x.Version).Where(x => x.ChangeType.Equals("publish")).ToList();

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

        foreach (var contentItem in contentItems)
        {
            var version = versions.FirstOrDefault(x => x.ItemId == contentItem.Id);

            if (version != null)
            {
                contentItem.Owner = version.Owner;
                contentItem.ChangeType = version.ChangeType;
            }
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
