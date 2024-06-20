using Progress.Sitefinity.RestSdk;
using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Toolkit.Data.Abstractions;
/// <summary>
/// Base class for Sitefinity REST SDK providers. Ensures that the REST client is initialized.
/// </summary>
internal class RestSdkBase
{
    private readonly IRestClient restClient;
    public RestSdkBase(IRestClient restClient)
    {
        var args = new RequestArgs();

        restClient.Init(args).Wait();

        this.restClient = restClient;
    }

    protected IEnumerable<T> GetUsingBatches<T>(GetAllArgs getAllArgs) where T : SdkItem
    {
        var items = new List<T>();
        int skip = 0;
        int take = 50;

        getAllArgs.Skip = skip;
        getAllArgs.Take = take;

        var result = restClient.GetItems<T>(getAllArgs);

        while (result.Result.Items.Any())
        {
            items.AddRange(result.Result.Items);

            skip += take;
            getAllArgs.Skip = skip;

            result = restClient.GetItems<T>(getAllArgs);
        }

        return items;
    }
}
