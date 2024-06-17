using Progress.Sitefinity.RestSdk;

namespace Migration.Toolkit.Data.Abstractions;
/// <summary>
/// Base class for Sitefinity REST SDK providers. Ensures that the REST client is initialized.
/// </summary>
internal class RestSdkBase
{
    public RestSdkBase(IRestClient restClient)
    {
        var args = new RequestArgs();

        restClient.Init(args).Wait();
    }
}
