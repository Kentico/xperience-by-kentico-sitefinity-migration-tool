using Microsoft.EntityFrameworkCore;

using Migration.Tookit.Data.Models;
using Migration.Toolkit.Data.Abstractions;
using Migration.Toolkit.Data.Core.EF;
using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;

using Progress.Sitefinity.RestSdk;

namespace Migration.Toolkit.Data.Providers;
internal class MediaProvider(IRestClient restClient, IDbContextFactory<SitefinityContext> sitefinityContext) : RestSdkBase(restClient), IMediaProvider
{
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
        using var context = sitefinityContext.CreateDbContext();
        var mediaItems = context.MediaContent.ToList();

        var getAllArgs = new GetAllArgs
        {
            Type = RestClientContentTypes.Documents
        };

        var documents = GetUsingBatches<Media>(getAllArgs);

        foreach (var document in documents)
        {
            var foundDocument = mediaItems.Find(x => x.Id == document.Id);

            if (foundDocument != null)
            {
                document.LastModifiedBy = foundDocument.LastModifiedBy;
                document.CreatedBy = foundDocument.Owner;
            }
        }

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
        using var context = sitefinityContext.CreateDbContext();
        var mediaItems = context.MediaContent.ToList();

        var getAllArgs = new GetAllArgs
        {
            Type = RestClientContentTypes.Images
        };

        var images = GetUsingBatches<Image>(getAllArgs);

        foreach (var image in images)
        {
            var foundImage = mediaItems.Find(x => x.Id == image.Id);

            if (foundImage != null)
            {
                image.LastModifiedBy = foundImage.LastModifiedBy;
                image.CreatedBy = foundImage.Owner;
            }
        }

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
        using var context = sitefinityContext.CreateDbContext();
        var mediaItems = context.MediaContent.ToList();

        var getAllArgs = new GetAllArgs
        {
            Type = RestClientContentTypes.Videos
        };

        var videos = GetUsingBatches<Media>(getAllArgs);

        foreach (var video in videos)
        {
            var foundVideo = mediaItems.Find(x => x.Id == video.Id);

            if (foundVideo != null)
            {
                video.LastModifiedBy = foundVideo.LastModifiedBy;
                video.CreatedBy = foundVideo.Owner;
            }
        }

        return videos;
    }
}
