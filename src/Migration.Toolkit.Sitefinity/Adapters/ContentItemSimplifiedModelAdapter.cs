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
    protected override ContentItemSimplifiedModel? AdaptInternal(ContentItem source, ContentDependencies dependenciesModel)
    {
        var rootFolder = ContentFolderInfoProvider.ProviderObject.GetRoot();

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

        var pageConfig = configuration.PageContentTypes?.FirstOrDefault(x => dataClassModel.ClassName != null && dataClassModel.ClassName.Contains(x.TypeName));

        if (pageConfig == null)
        {
            return default;
        }

        if (pageConfig.PageTemplateType.Equals("Listing"))
        {
            var listingPage = dependenciesModel.WebPages?.Values.FirstOrDefault(x => x.PageData?.TreePath?.Equals(pageConfig.PageRootPath) ?? false);

            if (listingPage == null)
            {
                return default;
            }

            var pageData = new PageDataModel
            {
                ItemOrder = null,
                PageUrls = contentHelper.GetPageUrls(dependenciesModel, source, pageConfig.PageRootPath),
                PageGuid = source.Id,
                ParentGuid = listingPage.ContentItemGUID,
                TreePath = pageConfig.PageRootPath + source.ItemDefaultUrl
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

        if (pageConfig.PageTemplateType.Equals("Detail"))
        {
            var pageData = new PageDataModel
            {
                ItemOrder = null
            };

            var pageContentItem = new ContentItemSimplifiedModel
            {
                ContentItemGUID = source.Id,
                ContentTypeName = dataClassModel.ClassName,
                Name = contentHelper.GetName(source.Title, source.Id),
                LanguageData = languageData.ToList(),
                IsReusable = true,
                PageData = pageData
            };

            return pageContentItem;
        }

        return default;
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
