using CMS.Helpers;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Configuration;
using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Core.Helpers;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Adapters;
internal class WebPageModelAdapter(ILogger<WebPageModelAdapter> logger, IContentHelper contentHelper, ISiteProvider siteProvider, SitefinityDataConfiguration dataConfiguration) : UmtAdapterBaseWithDependencies<Page, ContentDependencies, ContentItemSimplifiedModel>(logger)
{
    protected override ContentItemSimplifiedModel? AdaptInternal(Page source, ContentDependencies dependenciesModel)
    {
        var sites = siteProvider.GetSites();
        var currentSite = sites.FirstOrDefault(x => (x.LiveUrl != null && x.LiveUrl.Equals(dataConfiguration.SitefinitySiteDomain)) || (x.StagingUrl != null && x.StagingUrl.Equals(dataConfiguration.SitefinitySiteDomain)));
        var channel = dependenciesModel.Channels.Values.FirstOrDefault(x => x.ChannelGUID.Equals(currentSite?.Id));

        if (channel == null)
        {
            logger.LogWarning("Channel not found for domain: {Domain}. Skipping content item {ItemDefaultUrl}.", dataConfiguration.SitefinitySiteDomain, source.UrlName);
            return default;
        }

        var pageNodeClass = dependenciesModel.DataClasses.Values.FirstOrDefault(x => x.ClassName != null && x.ClassName.Contains("PageNode"));

        if (pageNodeClass == null)
        {
            logger.LogWarning("Page Node Data class not found. Skipping content item {ItemDefaultUrl}.", source.UrlName);
            return default;
        }

        var users = dependenciesModel.Users;

        users.TryGetValue(ValidationHelper.GetGuid(source.Owner, Guid.Empty), out var createdByUser);

        var languageData = contentHelper.GetLanguageData(dependenciesModel.ContentLanguages.Values.Select(language => language.ContentLanguageName), source.Title, pageNodeClass, createdByUser, source);

        var pageData = new PageDataModel
        {
            ItemOrder = null,
            PageUrls =
            [
                new PageUrlModel
                {
                    UrlPath = source.ViewUrl.TrimStart('/'),
                    LanguageName = "en",
                    PathIsDraft = false
                }
            ],
            ParentGuid = ValidationHelper.GetGuid(source.ParentId, Guid.Empty),
            TreePath = source.ViewUrl
        };

        var pageContentItem = new ContentItemSimplifiedModel
        {
            ContentItemGUID = source.Id,
            ContentTypeName = pageNodeClass.ClassName,
            Name = $"{ValidationHelper.GetCodeName(source.Title)}-{source.Id}",
            LanguageData = languageData.ToList(),
            IsReusable = false,
            PageData = pageData,
            ChannelName = channel.ChannelName,
        };

        return pageContentItem;
    }
}
