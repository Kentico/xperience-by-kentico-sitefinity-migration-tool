using AngleSharp;
using AngleSharp.Html.Dom;

using CMS.ContentEngine;
using CMS.ContentEngine.Internal;
using CMS.Helpers;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Configuration;
using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core.Factories;
using Migration.Toolkit.Sitefinity.Core.Helpers;
using Migration.Toolkit.Sitefinity.FieldTypes;
using Migration.Toolkit.Sitefinity.Model;

using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Toolkit.Sitefinity.Helpers;
internal class ContentHelper(ILogger<ContentHelper> logger,
                                ITypeProvider typeProvider,
                                IFieldTypeFactory fieldTypeFactory,
                                ISiteProvider siteProvider,
                                SitefinityDataConfiguration dataConfiguration) : IContentHelper
{
    private IEnumerable<Site>? sites;

    public IEnumerable<ContentItemLanguageData> GetLanguageData(ContentDependencies contentDependencies, ICultureSdkItem cultureSdkItem, DataClassModel dataClassModel, UserInfoModel? createdByUser)
    {
        var languageData = new List<ContentItemLanguageData>();

        foreach (var culture in contentDependencies.ContentLanguages.Values)
        {
            if (culture.ContentLanguageIsDefault == null)
            {
                continue;
            }

            if (culture.ContentLanguageName == null)
            {
                continue;
            }

            if (ValidationHelper.GetBoolean(culture.ContentLanguageIsDefault, false))
            {
                var contentLanguageData = GetLanguageDataInternal(contentDependencies, culture.ContentLanguageName, cultureSdkItem, dataClassModel, createdByUser);

                if (contentLanguageData == null)
                {
                    logger.LogWarning("Failed to parse language data for default culture: {Culture}. Skipping content item {ItemDefaultUrl}.", culture.ContentLanguageName, cultureSdkItem.UrlName);
                    continue;
                }

                languageData.Add(contentLanguageData);
            }
            else
            {
                foreach (var alternateLanguageContentItem in cultureSdkItem.AlternateLanguageContentItems)
                {
                    if (alternateLanguageContentItem.Culture == null)
                    {
                        continue;
                    }

                    if (alternateLanguageContentItem.Culture.Equals(culture.ContentLanguageCultureFormat))
                    {
                        var contentLanguageData = GetLanguageDataInternal(contentDependencies, culture.ContentLanguageName, alternateLanguageContentItem, dataClassModel, createdByUser);

                        if (contentLanguageData == null)
                        {
                            logger.LogWarning("Failed to parse language data for alternate culture: {Culture}. Skipping content item {ItemDefaultUrl}.", culture.ContentLanguageName, alternateLanguageContentItem.UrlName);
                            continue;
                        }

                        languageData.Add(contentLanguageData);
                    }
                }
            }
        }

        return languageData;
    }

    private ContentItemLanguageData? GetLanguageDataInternal(ContentDependencies contentDependencies, string languageName, ICultureSdkItem cultureSdkItem, DataClassModel dataClassModel, UserInfoModel? user)
    {
        if (string.IsNullOrEmpty(cultureSdkItem.UrlName))
        {
            return default;
        }

        var types = typeProvider.GetAllTypes();

        var type = types.FirstOrDefault(x => x.Id == dataClassModel.ClassGUID);

        if (type == null || type.Fields == null)
        {
            return default;
        }

        var contentItemData = new Dictionary<string, object?>();

        foreach (var field in type.Fields)
        {
            var fieldType = fieldTypeFactory.CreateFieldType(field.WidgetTypeName);

            if (field.Name == null)
            {
                continue;
            }

            if (Constants.ExcludedFields.Contains(field.Name))
            {
                continue;
            }

            try
            {
                if (cultureSdkItem is SdkItem sdkItem)
                {
                    object data = fieldType.GetData(sdkItem, field.Name);

                    if (fieldType is HtmlFieldType)
                    {
                        data = UpdateImageUrls(contentDependencies, ValidationHelper.GetString(data, ""));
                    }

                    contentItemData.Add(field.Name, data);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Cannot get data for {FieldName} field.", field.Name);
            }
        }

        return new ContentItemLanguageData
        {
            DisplayName = cultureSdkItem.Title.Length > 100 ? cultureSdkItem.Title[..100] : cultureSdkItem.Title,
            LanguageName = languageName,
            UserGuid = user?.UserGUID,
            ContentItemData = contentItemData,
            VersionStatus = VersionStatus.Published,
        };
    }

    public string GetName(string title, Guid id, int length = 100)
    {
        string name = $"{ValidationHelper.GetCodeName(title)}-{id}";

        if (name.Length > length)
        {
            return name[..length];
        }

        return name;
    }

    public Site? GetCurrentSite()
    {
        sites ??= siteProvider.GetSites();
        return sites.FirstOrDefault(x => (x.LiveUrl != null && x.LiveUrl.Equals(dataConfiguration.SitefinitySiteDomain)) || (x.StagingUrl != null && x.StagingUrl.Equals(dataConfiguration.SitefinitySiteDomain)));
    }

    public ChannelModel? GetCurrentChannel(IEnumerable<ChannelModel> channels)
    {
        var currentSite = GetCurrentSite();
        return channels.FirstOrDefault(x => x.ChannelGUID.Equals(currentSite?.Id));
    }

    public List<PageUrlModel> GetPageUrls(ContentDependencies dependenciesModel, ICultureSdkItem source, string? rootPath = null, string? pagePath = null)
    {
        var pageUrls = new List<PageUrlModel>();

        string pageUrl = GetUrl(source, rootPath, pagePath);

        var currentSite = GetCurrentSite();

        if (currentSite == null)
        {
            logger.LogWarning("Current site not found. Cannot get page urls for {UrlName}.", source.UrlName);
            return pageUrls;
        }

        foreach (var siteCulture in currentSite.SystemCultures)
        {
            var culture = dependenciesModel.ContentLanguages.Values.FirstOrDefault(x => x.ContentLanguageCultureFormat == siteCulture.Culture);

            if (culture == null || string.IsNullOrEmpty(source.Url))
            {
                continue;
            }

            if (ValidationHelper.GetBoolean(culture.ContentLanguageIsDefault, false))
            {
                pageUrls.Add(new PageUrlModel
                {
                    UrlPath = pageUrl.TrimStart('/'),
                    LanguageName = culture.ContentLanguageName,
                    PathIsDraft = false
                });
            }
            else
            {
                var alternateLanguageContentItem = source.AlternateLanguageContentItems.Find(x => x.Culture == culture.ContentLanguageCultureFormat);

                if (alternateLanguageContentItem == null || string.IsNullOrEmpty(alternateLanguageContentItem.Url))
                {
                    pageUrls.Add(new PageUrlModel
                    {
                        UrlPath = culture.ContentLanguageName + pageUrl,
                        LanguageName = culture.ContentLanguageName,
                        PathIsDraft = false
                    });

                    continue;
                }

                pageUrls.Add(new PageUrlModel
                {
                    UrlPath = GetUrl(alternateLanguageContentItem, rootPath, pagePath).TrimStart('/'),
                    LanguageName = culture.ContentLanguageName,
                    PathIsDraft = false
                });
            }
        }

        return pageUrls;
    }

    private static string GetUrl(ICultureSdkItem source, string? rootPath, string? pagePath)
    {
        string pageUrl = source.Url;

        if (!string.IsNullOrEmpty(pagePath))
        {
            pageUrl = pagePath;
        }

        if (!string.IsNullOrEmpty(rootPath))
        {
            pageUrl = rootPath + pageUrl;
        }

        return pageUrl;
    }

    public string GetParentPath(string? path) => TreePathUtils.RemoveLastPathSegment(path);

    public string RemovePathSegmentsFromStart(string path, int numberOfSegments)
    {
        string[] segments = path.Split('/');
        int remainingSegments = segments.Length - numberOfSegments;
        if (remainingSegments <= 0)
        {
            return "";
        }
        string newPath = $"/{string.Join("/", segments.TakeLast(remainingSegments))}";
        return newPath;
    }

    public string UpdateImageUrls(ContentDependencies contentDependencies, string html)
    {
        int updatedSrcCount = 0;
        int updatedHrefCount = 0;
        string page = $"<html><body>{html}</body>";

        using var context = BrowsingContext.New();
        using var doc = context.OpenAsync(req => req.Content(page)).Result;
        foreach (var image in doc.Images)
        {
            if (image.Source == null)
            {
                continue;
            }

            string url = image.Source.Replace("http://localhost", "");
            string? permaLInkUrl = GetPermalink(contentDependencies, url);
            if (!string.Equals(url, permaLInkUrl, StringComparison.OrdinalIgnoreCase))
            {
                updatedSrcCount++;
            }
            image.Source = permaLInkUrl;
        }

        foreach (var link in doc.Links.ToList().OfType<IHtmlAnchorElement>())
        {
            if (link.Href == null)
            {
                continue;
            }

            string url = link.Href.Replace("http://localhost", "");
            string? permaLInkUrl = GetPermalink(contentDependencies, url);
            if (!string.Equals(url, permaLInkUrl, StringComparison.OrdinalIgnoreCase))
            {
                updatedHrefCount++;
            }

            if (permaLInkUrl != null)
            {
                link.Href = permaLInkUrl;
            }
        }

        if (updatedSrcCount > 0)
        {
            logger.LogInformation("\tPermalinks created {Count}", updatedSrcCount);
        }

        if (updatedHrefCount > 0)
        {
            logger.LogInformation("\tHrefs updated {Count}", updatedHrefCount);
        }

        if (doc.Body == null)
        {
            return html;
        }

        return doc.Body.InnerHtml;
    }

    private string? GetPermalink(ContentDependencies contentDependencies, string url)
    {
        url = url.TrimStart('~');

        if (url.StartsWith("tel:", StringComparison.OrdinalIgnoreCase) || url.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
        {
            return url;
        }

        MediaFileModel? mediaFile = null;

        if (Uri.TryCreate(url, UriKind.Absolute, out var absoluteUri))
        {
            mediaFile = contentDependencies.MediaFiles.Values.FirstOrDefault(x => x.DataSourceUrl == URLHelper.RemoveQuery(absoluteUri.ToString()));
        }

        if (Uri.TryCreate(url, UriKind.Relative, out var relativeUri))
        {
            mediaFile = contentDependencies.MediaFiles.Values.FirstOrDefault(x => x.DataSourceUrl == "https://" + dataConfiguration.SitefinitySiteDomain + URLHelper.RemoveQuery(url));
        }

        if (mediaFile == null)
        {
            logger.LogInformation("Could not find media file for {PathAndQuery}", url);
            return url;
        }

        return $"/getmedia/{mediaFile.FileGUID}/{mediaFile.FileName}{URLHelper.GetQuery(url)}";
    }
}
