using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Abstractions;
using Migration.Toolkit.Data.Core.EF;
using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;

using Progress.Sitefinity.RestSdk;

namespace Migration.Toolkit.Data.Providers;
internal class ContentProvider(IRestClient restClient, ILogger<ContentProvider> logger, IDbContextFactory<SitefinityContext> sitefinityContext) : RestSdkBase(restClient), IContentProvider
{
    private IEnumerable<SitefinityVersionChange>? versions;
    private IEnumerable<SitefinityPageNode>? pageNodes;

    public IEnumerable<ContentItem> GetContentItems(IEnumerable<SitefinityTypeDefinition> typeDefinitions, IEnumerable<SystemCulture> cultures)
    {
        using var context = sitefinityContext.CreateDbContext();
        versions ??= [.. context.VersionChanges.OrderByDescending(x => x.Version).Where(x => x.ChangeType.Equals("publish"))];

        var defaultCulture = cultures.FirstOrDefault(cultures => cultures.IsDefault);

        if (defaultCulture == null || defaultCulture.Culture == null)
        {
            logger.LogCritical("Default culture not found. Cannot retrieve content items from Sitefinity.");
            return [];
        }

        var contentItems = GetContentItemsInternal(typeDefinitions, defaultCulture);

        foreach (var alternateCulture in cultures.Where(x => !defaultCulture.Culture.Equals(x.Culture)))
        {
            var alternateCultureContentItems = GetContentItemsInternal(typeDefinitions, alternateCulture);

            foreach (var alternateContentItem in alternateCultureContentItems)
            {
                if (!contentItems.TryGetValue(alternateContentItem.Key, out var contentItem))
                {
                    continue;
                }

                contentItem.AlternateLanguageContentItems.Add(alternateContentItem.Value);
            }
        }

        return contentItems.Values;
    }

    private Dictionary<Guid, ContentItem> GetContentItemsInternal(IEnumerable<SitefinityTypeDefinition> typeDefinitions, SystemCulture defaultCulture)
    {
        var contentItems = new Dictionary<Guid, ContentItem>();

        foreach (var typeDefinition in typeDefinitions)
        {
            var getAllArgs = new GetAllArgs
            {
                Type = $"{typeDefinition.SitefinityTypeNameSpace}.{typeDefinition.SitefinityTypeName}",
                Fields = ["*"],
                Culture = defaultCulture.Culture
            };

            var items = GetUsingBatches<ContentItem>(getAllArgs);

            foreach (var item in items)
            {
                item.DataClassGuid = typeDefinition.DataClassGuid;
                item.TypeName = typeDefinition.SitefinityTypeName;

                contentItems.Add(item.Id, item);
            }
        }

        foreach (var contentItem in contentItems)
        {
            if (versions == null)
            {
                break;
            }

            var version = versions.FirstOrDefault(x => x.ItemId == contentItem.Key);

            if (version != null)
            {
                contentItem.Value.Owner = version.Owner;
                contentItem.Value.ChangeType = version.ChangeType;
            }

            contentItem.Value.Culture = defaultCulture.Culture;
        }

        return contentItems;
    }

    public IEnumerable<Page> GetPages(IEnumerable<SystemCulture> cultures)
    {
        using var context = sitefinityContext.CreateDbContext();
        pageNodes ??= [.. context.PageNodes];

        var defaultCulture = cultures.FirstOrDefault(cultures => cultures.IsDefault);

        if (defaultCulture == null || defaultCulture.Culture == null)
        {
            logger.LogCritical("Default culture not found. Cannot retrieve content items from Sitefinity.");
            return [];
        }

        var pages = GetPagesInternal(defaultCulture);

        foreach (var alternateCulture in cultures.Where(x => !defaultCulture.Culture.Equals(x.Culture)))
        {
            var alternateCultureContentItems = GetPagesInternal(alternateCulture);

            foreach (var alternateContentItem in alternateCultureContentItems)
            {
                if (!pages.TryGetValue(alternateContentItem.Key, out var contentItem))
                {
                    continue;
                }

                contentItem.AlternateLanguageContentItems.Add(alternateContentItem.Value);
            }
        }

        return pages.Values;
    }

    private Dictionary<Guid, Page> GetPagesInternal(SystemCulture culture)
    {
        var pages = new Dictionary<Guid, Page>();
        var getAllArgs = new GetAllArgs
        {
            Type = RestClientContentTypes.Pages,
            Culture = culture.Culture
        };

        var sitefinityPages = GetUsingBatches<Page>(getAllArgs);

        foreach (var page in sitefinityPages)
        {
            if (pageNodes == null)
            {
                break;
            }

            var pageNode = pageNodes.FirstOrDefault(x => x.Id == page.Id);

            if (pageNode != null)
            {
                page.Owner = pageNode.Owner;
            }

            pages.Add(page.Id, page);
        }

        return pages;
    }
}
