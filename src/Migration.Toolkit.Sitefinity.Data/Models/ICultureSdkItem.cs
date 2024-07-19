using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Toolkit.Data.Models;
public interface ICultureSdkItem : ISdkItem
{
    public string Title { get; set; }
    public string? Culture { get; set; }
    public string UrlName { get; set; }
    public string Url { get; }
    public List<ICultureSdkItem> AlternateLanguageContentItems { get; set; }
}
