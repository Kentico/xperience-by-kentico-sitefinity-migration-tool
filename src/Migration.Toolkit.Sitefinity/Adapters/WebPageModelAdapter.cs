﻿using CMS.Helpers;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Configuration;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Core.Helpers;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Adapters;
internal class WebPageModelAdapter(ILogger<WebPageModelAdapter> logger, IContentHelper contentHelper, SitefinityDataConfiguration dataConfiguration) : UmtAdapterBaseWithDependencies<Page, ContentDependencies, ContentItemSimplifiedModel>(logger)
{
    protected override ContentItemSimplifiedModel? AdaptInternal(Page source, ContentDependencies dependenciesModel)
    {
        var channel = contentHelper.GetCurrentChannel(dependenciesModel.Channels.Values);

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

        var languageData = contentHelper.GetLanguageData(dependenciesModel, source, pageNodeClass, createdByUser);

        var pageData = new PageDataModel
        {
            ItemOrder = null,
            PageUrls = contentHelper.GetPageUrls(dependenciesModel, source),
            PageGuid = source.Id,
            ParentGuid = ValidationHelper.GetGuid(source.ParentId, Guid.Empty),
            TreePath = contentHelper.GetRelativeUrl(source.ViewUrl)
        };

        var pageContentItem = new ContentItemSimplifiedModel
        {
            ContentItemGUID = source.Id,
            ContentTypeName = pageNodeClass.ClassName,
            Name = contentHelper.GetName(source.Title, source.Id),
            LanguageData = languageData.ToList(),
            IsReusable = false,
            PageData = pageData,
            ChannelName = channel.ChannelName,
        };

        return pageContentItem;
    }
}
