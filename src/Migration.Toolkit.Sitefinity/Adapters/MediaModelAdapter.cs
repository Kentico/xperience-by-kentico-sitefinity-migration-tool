using CMS.ContentEngine;

using CMS.Helpers;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Configuration;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Configuration;
using Migration.Toolkit.Sitefinity.Core.Helpers;
using Migration.Toolkit.Sitefinity.Helpers;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Adapters;

internal class MediaModelAdapter(ILogger<MediaModelAdapter> logger,
                                 SitefinityDataConfiguration sitefinityDataConfiguration,
                                 SitefinityImportConfiguration configuration,
                                 IContentHelper contentHelper) : UmtAdapterBaseWithDependencies<Media, MediaFileDependencies>(logger)
{
    protected override IEnumerable<IUmtModel>? AdaptInternal(Media sourceMediaItem, MediaFileDependencies mediaFileDependencies)
    {
        var adaptedContentItem = AdaptMediaItem(sourceMediaItem, mediaFileDependencies);
        return adaptedContentItem != null ? [adaptedContentItem] : null;
    }

    private ContentItemSimplifiedModel? AdaptMediaItem(Media sourceMediaItem, MediaFileDependencies mediaFileDependencies)
    {
        logger.LogInformation("=== PROCESSING MEDIA ITEM === ID: {Id}, Title: {Title}, UrlName: {UrlName}, ItemDefaultUrl: {ItemDefaultUrl}, Url: {Url}, Extension: {Extension}, TotalSize: {TotalSize}, Description: {Description}, CreatedBy: {CreatedBy} ===============================",
            sourceMediaItem.Id, sourceMediaItem.Title, sourceMediaItem.UrlName, sourceMediaItem.ItemDefaultUrl, sourceMediaItem.Url, sourceMediaItem.Extension, sourceMediaItem.TotalSize, sourceMediaItem.Description, sourceMediaItem.CreatedBy);

        // Validate media item URL
        if (string.IsNullOrEmpty(sourceMediaItem.ItemDefaultUrl))
        {
            logger.LogWarning("No default URL for media file. Skipping.");
            return default;
        }

        string[] urlPathParts = sourceMediaItem.ItemDefaultUrl.Trim('/').Split('/');

        if (urlPathParts.Length < 3)
        {
            logger.LogWarning("Invalid URL format for media file {ItemDefaultUrl}. Skipping.", sourceMediaItem.ItemDefaultUrl);
            return default;
        }

        var availableUsers = mediaFileDependencies.Users;
        availableUsers.TryGetValue(ValidationHelper.GetGuid(sourceMediaItem.CreatedBy, Guid.Empty), out var mediaCreatedByUser);

        // Determine content type based on media type
        string mediaContentTypeName = GetContentTypeName(sourceMediaItem, configuration.SitefinityCodeNamePrefix);

        var mediaDataClass = mediaFileDependencies.DataClasses.Values.FirstOrDefault(dataClass => dataClass.ClassName == mediaContentTypeName);

        if (mediaDataClass == null)
        {
            logger.LogWarning("Data class {ContentTypeName} not found. Skipping media file {ItemDefaultUrl}.", mediaContentTypeName, sourceMediaItem.ItemDefaultUrl);
            return default;
        }

        // Get default content language
        var defaultContentLanguage = mediaFileDependencies.ContentLanguages.Values.FirstOrDefault(languageItem => ValidationHelper.GetBoolean(languageItem.ContentLanguageIsDefault, false));
        if (defaultContentLanguage == null)
        {
            logger.LogWarning("Default content language not found. Skipping media file {ItemDefaultUrl}.", sourceMediaItem.ItemDefaultUrl);
            return default;
        }

        // Use the folder manager to get or create the organized folder
        var folderDependencies = new ContentFolderDependencies
        {
            ContentFolders = mediaFileDependencies.ContentFolders
        };
        var targetFolderGuid = mediaFileDependencies.FolderManager.GetOrCreateOrganizedFolder(sourceMediaItem, folderDependencies);

        logger.LogInformation("MEDIA ITEM: {ItemDefaultUrl} -> FOLDER GUID: {FolderGuid}",
            sourceMediaItem.ItemDefaultUrl, targetFolderGuid);

        // Create asset URL
        string mediaAssetUrl = Uri.IsWellFormedUriString(sourceMediaItem.Url, UriKind.Absolute)
            ? URLHelper.RemoveQuery(sourceMediaItem.Url)
            : "https://" + sitefinityDataConfiguration.SitefinitySiteDomain + URLHelper.RemoveQuery(sourceMediaItem.Url);

        // Create language data with the asset using the default language
        string displayName = !string.IsNullOrWhiteSpace(sourceMediaItem.Title) ? sourceMediaItem.Title : sourceMediaItem.UrlName;
        if (displayName.Length > 100)
        {
            displayName = displayName[..100];
        }

        Dictionary<string, object?>? contentItemData;
        try
        {
            contentItemData = CreateContentItemData(sourceMediaItem, mediaAssetUrl);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create content item data for media item {ItemId} ({ItemDefaultUrl})", sourceMediaItem.Id, sourceMediaItem.ItemDefaultUrl);
            return default;
        }

        var mediaLanguageData = new List<ContentItemLanguageData>
        {
            new()
            {
                LanguageName = defaultContentLanguage.ContentLanguageName ?? "en",
                DisplayName = displayName,
                VersionStatus = VersionStatus.Published,
                UserGuid = mediaCreatedByUser?.UserGUID,
                ContentItemData = contentItemData
            }
        };

        var adaptedContentItem = new ContentItemSimplifiedModel
        {
            ContentItemGUID = sourceMediaItem.Id,
            ContentTypeName = mediaContentTypeName,
            Name = contentHelper.GetName(!string.IsNullOrWhiteSpace(sourceMediaItem.Title) ? sourceMediaItem.Title : sourceMediaItem.UrlName, sourceMediaItem.Id),
            LanguageData = mediaLanguageData,
            IsReusable = true,
            ContentItemContentFolderGUID = targetFolderGuid
        };

        logger.LogInformation("Successfully created content item: {Name} with ContentTypeName: {ContentTypeName}",
            adaptedContentItem.Name, adaptedContentItem.ContentTypeName);
        return adaptedContentItem;
    }

    private static string GetContentTypeName(Media sourceMediaItem, string configuredCodeNamePrefix)
    {
        string contentType = sourceMediaItem switch
        {
            _ when MediaClassificationHelper.IsImage(sourceMediaItem) => $"{configuredCodeNamePrefix}.Image",
            _ when MediaClassificationHelper.IsVideo(sourceMediaItem) => $"{configuredCodeNamePrefix}.Video",
            _ when MediaClassificationHelper.IsAudio(sourceMediaItem) => $"{configuredCodeNamePrefix}.Video",
            _ when MediaClassificationHelper.IsDownload(sourceMediaItem) => $"{configuredCodeNamePrefix}.Download",
            _ => $"{configuredCodeNamePrefix}.Download" // Default fallback to Download
        };
        return contentType;
    }

    private Dictionary<string, object?> CreateContentItemData(Media sourceMediaItem, string constructedAssetUrl)
    {
        var contentItemDataDictionary = new Dictionary<string, object?>();

        var assetUrlSource = CreateAssetUrlSource(sourceMediaItem, constructedAssetUrl);
        string title = !string.IsNullOrWhiteSpace(sourceMediaItem.Title) ? sourceMediaItem.Title : sourceMediaItem.UrlName;
        string description = sourceMediaItem.Description ?? string.Empty;

        // Convert absolute URL to relative URL for legacy fields
        string relativeLegacyUrl = string.Empty;
        if (Uri.TryCreate(constructedAssetUrl, UriKind.Absolute, out var absoluteUri))
        {
            relativeLegacyUrl = URLHelper.RemoveQuery(absoluteUri.PathAndQuery);
        }
        else if (Uri.TryCreate(constructedAssetUrl, UriKind.Relative, out _))
        {
            relativeLegacyUrl = URLHelper.RemoveQuery(relativeLegacyUrl);
        }

        logger.LogInformation("Creating AssetUrlSource - ContentItemGuid: {ContentItemGuid}, Identifier: {Identifier}, Name: {Name}, Extension: {Extension}, Url: {Url}",
            assetUrlSource.ContentItemGuid, assetUrlSource.Identifier, assetUrlSource.Name, assetUrlSource.Extension, assetUrlSource.Url);

        if (MediaClassificationHelper.IsImage(sourceMediaItem))
        {
            contentItemDataDictionary["ImageTitle"] = title;
            contentItemDataDictionary["ImageDescription"] = description;
            contentItemDataDictionary["ImageAssetLegacyUrl"] = relativeLegacyUrl;
            contentItemDataDictionary["ImageAsset"] = assetUrlSource;
        }
        else if (MediaClassificationHelper.IsVideo(sourceMediaItem) || MediaClassificationHelper.IsAudio(sourceMediaItem))
        {
            contentItemDataDictionary["VideoTitle"] = title;
            contentItemDataDictionary["VideoDescription"] = description;
            contentItemDataDictionary["VideoAssetLegacyUrl"] = relativeLegacyUrl;
            contentItemDataDictionary["VideoAsset"] = assetUrlSource;
        }
        else // Default to Download
        {
            contentItemDataDictionary["DownloadTitle"] = title;
            contentItemDataDictionary["DownloadDescription"] = description;
            contentItemDataDictionary["DownloadAssetLegacyUrl"] = relativeLegacyUrl;
            contentItemDataDictionary["DownloadAsset"] = assetUrlSource;
        }

        return contentItemDataDictionary;
    }

    private static AssetUrlSource CreateAssetUrlSource(Media sourceMediaItem, string constructedAssetUrl)
    {
        // Validate the URL first
        if (string.IsNullOrEmpty(constructedAssetUrl) || !Uri.IsWellFormedUriString(constructedAssetUrl, UriKind.Absolute))
        {
            throw new ArgumentException($"Invalid asset URL for media item {sourceMediaItem.Id}: {constructedAssetUrl}");
        }

        var assetUrlSource = new AssetUrlSource
        {
            ContentItemGuid = sourceMediaItem.Id, // This should match the ContentItemSimplifiedModel.ContentItemGUID
            Identifier = Guid.NewGuid(), // Unique identifier for the asset itself
            Name = Path.GetFileName(constructedAssetUrl),
            Extension = Path.GetExtension(constructedAssetUrl),
            Url = constructedAssetUrl,
            LastModified = sourceMediaItem.LastModified
        };

        return assetUrlSource;
    }
}
