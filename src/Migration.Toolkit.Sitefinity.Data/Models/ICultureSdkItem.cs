using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model to represent a culture specific SDK item.
/// </summary>
public interface ICultureSdkItem : ISdkItem
{
    /// <summary>
    /// The title of the culture specific SDK item.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The culture of the culture specific SDK item.
    /// </summary>
    public string? Culture { get; set; }

    /// <summary>
    /// The URL name of the culture specific SDK item.
    /// </summary>
    public string UrlName { get; set; }

    /// <summary>
    /// The URL of the culture specific SDK item.
    /// </summary>
    public string Url { get; }

    /// <summary>
    /// The list of alternate language content items for the culture specific SDK item.
    /// </summary>
    public List<ICultureSdkItem> AlternateLanguageContentItems { get; set; }
}
