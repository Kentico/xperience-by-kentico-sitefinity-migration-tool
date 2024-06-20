using CMS.Helpers;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Tookit.Data.Models;
using Migration.Toolkit.Data.Configuration;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Adapters;
internal class MediaModelAdapter(ILogger<MediaLibraryModelAdapter> logger, SitefinityDataConfiguration sitefinityDataConfiguration) : UmtAdapterBase<Media, MediaFileDependencies, MediaFileModel>(logger)
{
    protected override MediaFileModel? AdaptInternal(Media source, MediaFileDependencies mediaFileDependencies)
    {
        var mediaLibraries = mediaFileDependencies.MediaLibraries;

        if (!mediaLibraries.TryGetValue(ValidationHelper.GetGuid(source.ParentId, Guid.Empty), out var library))
        {
            logger.LogWarning($"Media library with GUID {source.ParentId} not found. Skipping media file {source.ItemDefaultUrl}.");
            return default;
        }

        var users = mediaFileDependencies.Users;

        users.TryGetValue(ValidationHelper.GetGuid(source.CreatedBy, Guid.Empty), out var createdByUser);
        users.TryGetValue(ValidationHelper.GetGuid(source.LastModifiedBy, Guid.Empty), out var modifiedByUser);

        var uri = new Uri(sitefinityDataConfiguration.SitefinitySiteUrl + source.ItemDefaultUrl, UriKind.Absolute);

        string mediaPath = uri.Segments.Skip(4).SkipLast(1).Join("/");

        var mediaFile = new MediaFileModel
        {
            FileName = source.UrlName,
            FileDescription = source.Description,
            FileMimeType = source.MimeType,
            FileGUID = source.Id,
            FileModifiedWhen = source.LastModified,
            FileTitle = source.Title,
            FilePath = mediaPath,
            FileExtension = source.Extension,
            FileLibraryGuid = library.LibraryGUID,
            FileCreatedWhen = source.DateCreated,
            FileModifiedByUserGuid = modifiedByUser?.UserGUID,
            FileCreatedByUserGuid = createdByUser?.UserGUID,
            DataSourceUrl = Uri.IsWellFormedUriString(source.Url, UriKind.Absolute)
            ? source.Url
            : sitefinityDataConfiguration.SitefinitySiteUrl + source.Url,
        };

        if (source is Image image)
        {
            mediaFile.FileImageWidth = image.Width;
            mediaFile.FileImageHeight = image.Height;
        }

        return mediaFile;
    }
}
