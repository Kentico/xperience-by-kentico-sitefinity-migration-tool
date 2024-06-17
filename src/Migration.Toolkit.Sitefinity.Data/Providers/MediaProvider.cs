using Migration.Toolkit.Data.Abstractions;
using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;

using Progress.Sitefinity.RestSdk;

namespace Migration.Toolkit.Data.Providers;
internal class MediaProvider(IRestClient restClient) : RestSdkBase(restClient), IMediaProvider
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

    public IEnumerable<Library> GetImageLibraries()
    {
        var getAllArgs = new GetAllArgs
        {
            Type = "Telerik.Sitefinity.Libraries.Model.Album"
        };

        var libraries = restClient.GetItems<Library>(getAllArgs);

        return libraries.Result.Items;
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
}
