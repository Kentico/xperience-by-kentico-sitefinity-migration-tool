using CMS.ContentEngine;
using CMS.Helpers;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Configuration;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Configuration;
using Migration.Toolkit.Sitefinity.Core.Helpers;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Adapters;
internal class ContentItemSimplifiedModelAdapter(ILogger<ContentItemSimplifiedModelAdapter> logger, IContentHelper contentHelper, SitefinityImportConfiguration configuration, SitefinityDataConfiguration dataConfiguration) : UmtAdapterBaseWithDependencies<ContentItem, ContentDependencies, ContentItemSimplifiedModel>(logger)
{
    private readonly Dictionary<Guid, ContentItemSimplifiedModel> detailContentItems = [];

    protected override ContentItemSimplifiedModel? AdaptInternal(ContentItem source, ContentDependencies dependenciesModel)
    {
        var rootFolder = ContentFolderInfo.Provider.GetRootAsync(configuration.KenticoWorkspaceName).GetAwaiter().GetResult();

        if (rootFolder == null)
        {
            logger.LogWarning("Content Hub root folder not found.");
            return default;
        }

        if (!dependenciesModel.DataClasses.TryGetValue(source.DataClassGuid, out var dataClassModel))
        {
            logger.LogWarning("Data class with ClassGuid of {DataClassGuid} not found. Skipping content item {ItemDefaultUrl}.", source.DataClassGuid, source.ItemDefaultUrl);
            return default;
        }

        var users = dependenciesModel.Users;

        users.TryGetValue(ValidationHelper.GetGuid(source.Owner, Guid.Empty), out var createdByUser);

        var languageData = contentHelper.GetLanguageData(dependenciesModel, source, dataClassModel, createdByUser);

        if (dataClassModel.ClassContentTypeType == null)
        {
            return AdaptReusable(source, dataClassModel, languageData, rootFolder);
        }

        if (dataClassModel.ClassContentTypeType.Equals("Reusable"))
        {
            return AdaptReusable(source, dataClassModel, languageData, rootFolder);
        }

        if (dataClassModel.ClassContentTypeType.Equals("Website"))
        {
            return AdaptPage(source, dataClassModel, languageData, dependenciesModel);
        }

        return AdaptReusable(source, dataClassModel, languageData, rootFolder);
    }

    private ContentItemSimplifiedModel? AdaptPage(ContentItem source, DataClassModel dataClassModel, IEnumerable<ContentItemLanguageData> languageData, ContentDependencies dependenciesModel)
    {
        var channel = contentHelper.GetCurrentChannel(dependenciesModel.Channels.Values);

        if (channel == null)
        {
            logger.LogWarning("Channel not found for domain: {Domain}. Skipping content item {ItemDefaultUrl}.", dataConfiguration.SitefinitySiteDomain, source.UrlName);
            return default;
        }

        var pageConfigs = configuration.PageContentTypes?.Where(x => dataClassModel.ClassName != null && dataClassModel.ClassName.Contains(x.TypeName));

        if (pageConfigs == null || !pageConfigs.Any())
        {
            var parentGuid = ValidationHelper.GetGuid(source.ParentId, Guid.Empty);
            string treePath = source.ItemDefaultUrl;
            string? pagePath = null;

            if (detailContentItems.TryGetValue(parentGuid, out var detailContentItem))
            {
                parentGuid = ValidationHelper.GetGuid(detailContentItem.ContentItemGUID, Guid.Empty);
                treePath = detailContentItem.PageData?.TreePath + contentHelper.RemovePathSegmentsFromStart(source.ItemDefaultUrl, 2);
                pagePath = treePath;
            }

            var noPageConfigPageData = new PageDataModel
            {
                ItemOrder = null,
                PageUrls = contentHelper.GetPageUrls(dependenciesModel, source, pagePath: pagePath),
                PageGuid = source.Id,
                ParentGuid = parentGuid,
                TreePath = treePath
            };

            var noPageConfigPageContentItem = new ContentItemSimplifiedModel
            {
                ContentItemGUID = source.Id,
                ContentTypeName = dataClassModel.ClassName,
                Name = contentHelper.GetName(source.Title, source.Id),
                LanguageData = languageData.ToList(),
                IsReusable = false,
                PageData = noPageConfigPageData,
                ChannelName = channel.ChannelName
            };

            return noPageConfigPageContentItem;
        }

        foreach (var pageConfig in pageConfigs)
        {
            if (pageConfig.PageTemplateType == PageTemplateType.Listing)
            {
                var listingPage = dependenciesModel.WebPages?.Values.FirstOrDefault(x => x.PageData?.TreePath?.Equals(pageConfig.PageRootPath) ?? false);

                if (listingPage == null)
                {
                    return default;
                }

                var listingChildPageData = new PageDataModel
                {
                    ItemOrder = null,
                    PageUrls = contentHelper.GetPageUrls(dependenciesModel, source, pageConfig.PageRootPath),
                    PageGuid = source.Id,
                    ParentGuid = listingPage.ContentItemGUID,
                    TreePath = pageConfig.PageRootPath + source.ItemDefaultUrl
                };

                var listingChildPageContentItem = new ContentItemSimplifiedModel
                {
                    ContentItemGUID = source.Id,
                    ContentTypeName = dataClassModel.ClassName,
                    Name = contentHelper.GetName(source.Title, source.Id),
                    LanguageData = languageData.ToList(),
                    IsReusable = false,
                    PageData = listingChildPageData,
                    ChannelName = channel.ChannelName
                };

                return listingChildPageContentItem;
            }

            if (pageConfig.PageTemplateType == PageTemplateType.Detail && (pageConfig.PageRootPath.Equals(source.Url) || (pageConfig.ItemUrlName != null && pageConfig.ItemUrlName.Equals(source.Url))))
            {
                var detailPage = dependenciesModel.WebPages?.Values.FirstOrDefault(x => (x.PageData?.TreePath?.Equals(pageConfig.PageRootPath) ?? false) || (x.PageData?.TreePath?.Equals(pageConfig.ItemUrlName) ?? false));

                if (detailPage == null)
                {
                    return default;
                }

                var parentDetailPage = dependenciesModel.WebPages?.Values.FirstOrDefault(x => x.PageData?.TreePath?.Equals(contentHelper.GetParentPath(detailPage.PageData?.TreePath)) ?? false);

                if (parentDetailPage == null)
                {
                    return default;
                }

                detailPage.ContentTypeName = dataClassModel.ClassName;
                detailPage.LanguageData = languageData.ToList();

                detailContentItems.Add(source.Id, detailPage);

                return detailPage;
            }
        }

        var pageData = new PageDataModel
        {
            ItemOrder = null,
            PageUrls = contentHelper.GetPageUrls(dependenciesModel, source),
            PageGuid = source.Id,
            ParentGuid = ValidationHelper.GetGuid(source.ParentId, Guid.Empty),
            TreePath = source.ItemDefaultUrl
        };

        var pageContentItem = new ContentItemSimplifiedModel
        {
            ContentItemGUID = source.Id,
            ContentTypeName = dataClassModel.ClassName,
            Name = contentHelper.GetName(source.Title, source.Id),
            LanguageData = languageData.ToList(),
            IsReusable = false,
            PageData = pageData,
            ChannelName = channel.ChannelName
        };

        return pageContentItem;
    }

    private ContentItemSimplifiedModel AdaptReusable(ContentItem source, DataClassModel dataClassModel, IEnumerable<ContentItemLanguageData> languageData, ContentFolderInfo rootFolder) => new()
    {
        ContentItemGUID = source.Id,
        ContentTypeName = dataClassModel.ClassName,
        Name = contentHelper.GetName(source.Title, source.Id),
        LanguageData = languageData.ToList(),
        IsReusable = true,
        ContentItemContentFolderGUID = rootFolder.ContentFolderGUID,
    };
}
