using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Data.Abstractions;
using Migration.Toolkit.Data.Core.EF;
using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;

using Progress.Sitefinity.RestSdk;

namespace Migration.Toolkit.Data.Providers;
internal class MediaProvider(IRestClient restClient, IDbContextFactory<SitefinityContext> sitefinityContext, ILogger<MediaProvider> logger) : RestSdkBase(restClient), IMediaProvider
{
    private Dictionary<Guid, SitefinityMediaContent>? mediaItems;
    public IEnumerable<Library> GetDocumentLibraries()
    {
        var getAllArgs = new GetAllArgs
        {
            Type = RestClientContentTypes.Libraries
        };

        var libraries = restClient.GetItems<Library>(getAllArgs);

        return libraries.Result.Items;
    }

    public IEnumerable<Media> GetDocuments()
    {
        var getAllArgs = new GetAllArgs
        {
            Type = RestClientContentTypes.Documents
        };

        var documents = GetUsingBatches<Media>(getAllArgs);

        SetUserInfo(documents);

        return documents;
    }

    public IEnumerable<Library> GetImageLibraries()
    {
        var getAllArgs = new GetAllArgs
        {
            Type = "Telerik.Sitefinity.Libraries.Model.Album"
        };

        var libraries = restClient.GetItems<Library>(getAllArgs);

        return libraries.Result.Items;
    }

    public IEnumerable<Media> GetImages()
    {
        var getAllArgs = new GetAllArgs
        {
            Type = RestClientContentTypes.Images
        };

        var images = GetUsingBatches<Image>(getAllArgs);

        SetUserInfo(images);

        return images;
    }

    public IEnumerable<Library> GetVideoLibraries()
    {
        var getAllArgs = new GetAllArgs
        {
            Type = "Telerik.Sitefinity.Libraries.Model.VideoLibrary"
        };

        var libraries = restClient.GetItems<Library>(getAllArgs);

        return libraries.Result.Items;
    }

    public IEnumerable<Media> GetVideos()
    {
        var getAllArgs = new GetAllArgs
        {
            Type = RestClientContentTypes.Videos
        };

        var videos = GetUsingBatches<Media>(getAllArgs);

        SetUserInfo(videos);

        return videos;
    }

    private void SetUserInfo(IEnumerable<Media> restMediaItems)
    {
        if (mediaItems == null)
        {
            using var context = sitefinityContext.CreateDbContext();
            mediaItems = context.MediaContent.ToDictionary(x => x.Id);
        }

        foreach (var restMediaItem in restMediaItems)
        {
            if (mediaItems.TryGetValue(restMediaItem.Id, out var foundDocument))
            {
                restMediaItem.LastModifiedBy = foundDocument.LastModifiedBy;
                restMediaItem.CreatedBy = foundDocument.Owner;
            }
            else
            {
                logger.LogWarning($"Document with id {restMediaItem.Id} not found in Sitefinity database. Could not add LastModifiedBy or Owner");
            }
        }
    }
}
