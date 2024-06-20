using CMS.Helpers;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Tookit.Data.Models;
using Migration.Toolkit.Data.Configuration;
using Migration.Toolkit.Sitefinity.Abstractions;

namespace Migration.Toolkit.Sitefinity.Adapters;
internal class MediaModelAdapter(ILogger<MediaLibraryModelAdapter> logger, SitefinityDataConfiguration sitefinityDataConfiguration) : UmtAdapterBase<Media, MediaFileModel>(logger)
{
    protected override MediaFileModel AdaptInternal(Media source)
    {
        var uri = new Uri(sitefinityDataConfiguration.SitefinitySiteUrl + source.ItemDefaultUrl, UriKind.Absolute);

        string mediaPath = uri.Segments.Skip(4).Join("/");

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
            FileLibraryGuid = ValidationHelper.GetGuid(source.ParentId, Guid.Empty),
            FileCreatedWhen = source.DateCreated,
            FileModifiedByUserGuid = ValidationHelper.GetGuid(source.LastModifiedBy, Guid.Empty),
            FileCreatedByUserGuid = ValidationHelper.GetGuid(source.CreatedBy, Guid.Empty),
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
