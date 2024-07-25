using CMS.Helpers;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;

namespace Migration.Toolkit.Sitefinity.Adapters;
internal class MediaLibraryModelAdapter(ILogger<MediaLibraryModelAdapter> logger) : UmtAdapterBase<Library, MediaLibraryModel>(logger)
{
    protected override MediaLibraryModel AdaptInternal(Library source) => new()
    {
        LibraryName = ValidationHelper.GetCodeName(source.UrlName),
        LibraryDescription = source.Description,
        LibraryGUID = source.Id,
        LibraryDisplayName = source.Title,
        LibraryFolder = source.UrlName,
        LibraryLastModified = source.LastModified,
    };
}
