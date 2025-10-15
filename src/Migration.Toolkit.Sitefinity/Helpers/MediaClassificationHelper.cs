

using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Sitefinity.Helpers;

/// <summary>
/// Provides helper methods for classifying media items by type based on file extension.
/// </summary>
internal static class MediaClassificationHelper
{
    /// <summary>
    /// Determines whether the media item is an image based on its file extension.
    /// </summary>
    /// <param name="mediaItem">The media item to classify.</param>
    /// <returns>True if the media item is an image; otherwise, false.</returns>
    internal static bool IsImage(Media mediaItem)
    {
        string[] imageFileExtensions = [".bmp", ".gif", ".ico", ".jpg", ".jpeg", ".png", ".svg", ".tif", ".tiff", ".webp", ".wmf", ".svg"];
        bool isImage = imageFileExtensions.Contains(mediaItem.Extension?.ToLowerInvariant());
        return isImage;
    }

    /// <summary>
    /// Determines whether the media item is a video based on its file extension.
    /// </summary>
    /// <param name="mediaItem">The media item to classify.</param>
    /// <returns>True if the media item is a video; otherwise, false.</returns>
    internal static bool IsVideo(Media mediaItem)
    {
        string[] videoFileExtensions = [".3g2", ".3gp", ".asf", ".avi", ".flv", ".m4v", ".mkv", ".mov", ".mp4", ".mpeg", ".mpg", ".ogv", ".swf", ".webm", ".wmv"];
        bool isVideo = videoFileExtensions.Contains(mediaItem.Extension?.ToLowerInvariant());
        return isVideo;
    }

    /// <summary>
    /// Determines whether the media item is an audio file based on its file extension.
    /// </summary>
    /// <param name="mediaItem">The media item to classify.</param>
    /// <returns>True if the media item is an audio file; otherwise, false.</returns>
    internal static bool IsAudio(Media mediaItem)
    {
        string[] audioFileExtensions = [".mid", ".midi", ".mp2", ".mp3", ".mpga", ".ogg", ".wav", ".wma"];
        bool isAudio = audioFileExtensions.Contains(mediaItem.Extension?.ToLowerInvariant());
        return isAudio;
    }

    /// <summary>
    /// Determines whether the media item is a downloadable document based on its file extension.
    /// </summary>
    /// <param name="mediaItem">The media item to classify.</param>
    /// <returns>True if the media item is a downloadable document; otherwise, false.</returns>
    internal static bool IsDownload(Media mediaItem)
    {
        string[] downloadFileExtensions = [".7z", ".csv", ".deb", ".dmg", ".doc", ".docx", ".exe", ".gz", ".msg", ".msi", ".odp", ".ods", ".odt", ".pdf", ".pps", ".ppsx", ".ppt", ".pptx", ".rar", ".rpm", ".rtf", ".tar", ".txt", ".wpd", ".xls", ".xlsx", ".xml", ".xps", ".zip"];
        bool isDownload = downloadFileExtensions.Contains(mediaItem.Extension?.ToLowerInvariant());
        return isDownload;
    }

}
