using Progress.Sitefinity.RestSdk;
using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Toolkit.Data.Abstractions;
/// <summary>
/// Base class for Sitefinity REST SDK providers. Ensures that the REST client is initialized.
/// </summary>
internal abstract class RestSdkBase
{
    protected readonly IRestClient RestClient;
    protected RestSdkBase(IRestClient restClient)
    {
        var args = new RequestArgs();
        try
        {
            restClient.Init(args).Wait();
        }
        catch
        {
            throw new InvalidOperationException("Failed to initialize REST client. Please ensure Sitefinity site is running and web services have been turned on.");
        }

        RestClient = restClient;
    }

    protected IEnumerable<T> GetUsingBatches<T>(GetAllArgs getAllArgs) where T : SdkItem
    {
        var items = new List<T>();
        int skip = 0;
        int take = 50;

        getAllArgs.Skip = skip;
        getAllArgs.Take = take;

        var result = RestClient.GetItems<T>(getAllArgs);

        while (result.Result.Items.Any())
        {
            items.AddRange(result.Result.Items);

            skip += take;
            getAllArgs.Skip = skip;

            result = RestClient.GetItems<T>(getAllArgs);
        }

        return items;
    }
}
