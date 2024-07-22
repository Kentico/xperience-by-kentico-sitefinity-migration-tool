using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Core.Helpers;
internal interface IContentHelper
{
    public IEnumerable<ContentItemLanguageData> GetLanguageData(ContentDependencies contentDependencies, ICultureSdkItem cultureSdkItem, DataClassModel dataClassModel, UserInfoModel? createdByUser);
    public string GetName(string title, Guid id, int length = 100);
    public Site? GetCurrentSite();
    public ChannelModel? GetCurrentChannel(IEnumerable<ChannelModel> channels);
    public List<PageUrlModel> GetPageUrls(ContentDependencies dependenciesModel, ICultureSdkItem source, string? rootPath = null, string? pagePath = null);
    public string GetParentPath(string? path);
    public string RemovePathSegmentsFromStart(string path, int numberOfSegments);
}
