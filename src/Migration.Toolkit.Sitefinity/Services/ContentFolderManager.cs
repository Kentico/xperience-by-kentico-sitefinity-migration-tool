using System.Security.Cryptography;

using CMS.Helpers;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Helpers;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Services;

/// <summary>
/// Service for managing content folder creation and tracking.
/// </summary>
internal class ContentFolderManager(ILogger<ContentFolderManager> logger)
{
    // Track created folder paths to avoid duplicates
    private readonly Dictionary<string, ContentFolderModel> createdFoldersDictionary = [];

    /// <summary>
    /// Gets or creates a folder for organizing media based on the media type and path.
    /// </summary>
    /// <param name="sourceMediaItem">The source media item</param>
    /// <param name="dependencies">The content folder dependencies</param>
    /// <returns>The GUID of the target folder</returns>
    public Guid GetOrCreateOrganizedFolder(Media sourceMediaItem, ContentFolderDependencies dependencies)
    {
        // Determine media type and create root content type folder
        string mediaTypeFolderName = GetRootMediaTypeFolderName(sourceMediaItem);

        // Create or get the root media type folder
        var rootMediaTypeFolder = GetOrCreateRootFolder(mediaTypeFolderName, dependencies.ContentFolders);

        // Extract subfolder path from the original Sitefinity URL
        string subfolderPath = ExtractSubfolderPath(sourceMediaItem);

        if (string.IsNullOrEmpty(subfolderPath))
        {
            return rootMediaTypeFolder.ContentFolderGUID ?? Guid.Empty;
        }

        // Create nested subfolder structure under the root media type folder
        var finalFolderGuid = CreateSubfolderStructure(subfolderPath, rootMediaTypeFolder, dependencies.ContentFolders);

        return finalFolderGuid;
    }

    /// <summary>
    /// Gets all folders that have been created during the current session.
    /// </summary>
    /// <returns>Collection of created folders</returns>
    public IEnumerable<ContentFolderModel> GetAllCreatedFolders() => createdFoldersDictionary.Values;

    /// <summary>
    /// Clears the created folders tracking dictionary.
    /// </summary>
    public void ClearCreatedFolders() => createdFoldersDictionary.Clear();

    private ContentFolderModel GetOrCreateRootFolder(string rootFolderName, IDictionary<Guid, ContentFolderModel> allAvailableFolders)
    {
        string rootFolderPath = $"/{rootFolderName}";
        string folderKeyForLookup = rootFolderPath.ToLowerInvariant();

        if (createdFoldersDictionary.TryGetValue(folderKeyForLookup, out var existingFolderModel))
        {
            return existingFolderModel;
        }

        // Create globally unique code name for root folder within 50 character limit
        string globallyUniqueCodeName = GenerateUniqueCodeName(rootFolderPath, $"root_{rootFolderName}");

        // Create new root folder
        var newRootFolder = new ContentFolderModel
        {
            ContentFolderGUID = Guid.NewGuid(),
            ContentFolderDisplayName = rootFolderName,
            ContentFolderName = globallyUniqueCodeName, // Use globally unique code name within 50 char limit
            ContentFolderTreePath = rootFolderPath,
            ContentFolderParentFolderGUID = null // Root level folder
        };

        createdFoldersDictionary[folderKeyForLookup] = newRootFolder;
        allAvailableFolders[newRootFolder.ContentFolderGUID ?? Guid.Empty] = newRootFolder;

        return newRootFolder;
    }

    private static string GetRootMediaTypeFolderName(Media sourceMediaItem)
    {
        string folderName = sourceMediaItem switch
        {
            _ when MediaClassificationHelper.IsImage(sourceMediaItem) => "images",
            _ when MediaClassificationHelper.IsVideo(sourceMediaItem) => "videos",
            _ when MediaClassificationHelper.IsAudio(sourceMediaItem) => "videos", // Audio goes to video folder
            _ when MediaClassificationHelper.IsDownload(sourceMediaItem) => "downloads",
            _ => "downloads" // Default fallback
        };
        return folderName;
    }

    private static string ExtractSubfolderPath(Media sourceMediaItem)
    {
        // Extract subfolder path from source URL, preserving the Sitefinity folder structure
        // Example: "/images/default-source/banners/industry/image.jpg" ? "banners/industry"
        // Example: "/documents/default-source/legal/document.pdf" ? "legal"
        // Example: "/images/default-source/default-library/screen-shot-2016-07-08-at-9-32-37-am" ? "default-library"
        if (string.IsNullOrEmpty(sourceMediaItem.ItemDefaultUrl))
        {
            return string.Empty;
        }

        string[] urlPathParts = sourceMediaItem.ItemDefaultUrl.Trim('/').Split('/');

        // URL structure: /[media-type]/[library-name]/[subfolders...]/[filename-without-extension]
        // We need at least 4 parts to have subfolders: media-type, library-name, subfolder, filename
        // Skip the first two segments (media type and library name) and process the rest
        if (urlPathParts.Length < 4)
        {
            return string.Empty;
        }

        // Skip the first two segments (media type and library name) and take everything except the last segment (which is always the filename)
        string[] pathSegmentsAfterLibrary = [.. urlPathParts.Skip(2)];

        // In Sitefinity URLs, the last segment is typically the filename without extension
        // So we should always exclude the last segment from the folder structure
        string[] subfolderSegments;
        if (pathSegmentsAfterLibrary.Length > 1)
        {
            // Remove the last segment (filename) from the segments
            subfolderSegments = [.. pathSegmentsAfterLibrary.Take(pathSegmentsAfterLibrary.Length - 1)];
        }
        else
        {
            // Only one segment after library, which means no subfolders, just filename
            subfolderSegments = [];
        }

        if (subfolderSegments.Length == 0)
        {
            return string.Empty;
        }

        string subfolderPath = string.Join("/", subfolderSegments);

        return subfolderPath;
    }

    private Guid CreateSubfolderStructure(string subfolderPath, ContentFolderModel parentFolder, IDictionary<Guid, ContentFolderModel> allAvailableFolders)
    {
        logger.LogDebug("Creating subfolders for path: '{SubfolderPath}' under parent: {ParentPath}", subfolderPath, parentFolder.ContentFolderTreePath);

        string[] pathSegments = subfolderPath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        var currentParentFolder = parentFolder;

        for (int i = 0; i < pathSegments.Length; i++)
        {
            string pathSegment = pathSegments[i];
            currentParentFolder = GetOrCreateSubfolder(pathSegment, currentParentFolder, allAvailableFolders);
        }

        logger.LogDebug("Final folder: {FinalPath} (GUID: {FolderGuid})", currentParentFolder.ContentFolderTreePath, currentParentFolder.ContentFolderGUID);
        return currentParentFolder.ContentFolderGUID ?? Guid.Empty;
    }

    private ContentFolderModel GetOrCreateSubfolder(string folderName, ContentFolderModel parentFolder, IDictionary<Guid, ContentFolderModel> allAvailableFolders)
    {
        // Create a globally unique code name by incorporating the full folder path
        // This ensures that folders with the same name under different parents have unique code names
        string fullFolderPath = $"{parentFolder.ContentFolderTreePath}/{folderName}";

        // Generate a globally unique code name that respects Kentico's 50-character limit
        string globallyUniqueCodeName = GenerateUniqueCodeName(fullFolderPath, folderName);

        string folderKeyForLookup = fullFolderPath.ToLowerInvariant();

        if (createdFoldersDictionary.TryGetValue(folderKeyForLookup, out var existingFolder))
        {
            return existingFolder;
        }

        // Create new subfolder with generated GUID and parent reference
        var newSubfolder = new ContentFolderModel
        {
            ContentFolderGUID = Guid.NewGuid(),
            ContentFolderDisplayName = folderName, // Keep original display name
            ContentFolderName = globallyUniqueCodeName, // Use globally unique code name within 50 char limit
            ContentFolderTreePath = fullFolderPath,
            ContentFolderParentFolderGUID = parentFolder.ContentFolderGUID
        };

        createdFoldersDictionary[folderKeyForLookup] = newSubfolder;
        allAvailableFolders[newSubfolder.ContentFolderGUID ?? Guid.Empty] = newSubfolder;

        return newSubfolder;
    }

    private static string GenerateUniqueCodeName(string fullFolderPath, string folderName)
    {
        // Create a hash of the full path to ensure uniqueness
        byte[] hashBytes = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(fullFolderPath));
        string pathHash = Convert.ToHexString(hashBytes)[..8]; // Use first 8 characters of hash

        // Sanitize the folder name
        string sanitizedFolderName = ValidationHelper.GetCodeName(folderName).Replace(".", "-");
        if (string.IsNullOrEmpty(sanitizedFolderName))
        {
            sanitizedFolderName = "folder"; // fallback name
        }

        // Calculate max length for folder name part (50 - 1 underscore - 8 hash = 41)
        const int maxFolderNameLength = 41;

        if (sanitizedFolderName.Length > maxFolderNameLength)
        {
            sanitizedFolderName = sanitizedFolderName[..maxFolderNameLength];
        }

        // Combine sanitized name with hash to ensure uniqueness within 50 character limit
        string uniqueCodeName = $"{sanitizedFolderName}_{pathHash}";

        // Final safety check to ensure we never exceed 50 characters
        if (uniqueCodeName.Length > 50)
        {
            // This should never happen with our calculation, but safety first
            uniqueCodeName = uniqueCodeName[..50];
        }

        return uniqueCodeName;
    }
}
