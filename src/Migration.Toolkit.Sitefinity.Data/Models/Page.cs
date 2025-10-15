using System.Text.Json.Serialization;

using Progress.Sitefinity.RestSdk.Clients.Pages.Dto;

namespace Migration.Toolkit.Data.Models;
/// <summary>
/// SdkItem model for Sitefinity pages.
/// </summary>
public class Page : PageNodeDto, ISitefinityModel, ICultureSdkItem
{
    /// <summary>
    /// The unique identifier of the page.
    /// </summary>
    public new Guid Id
    {
        get
        {
            if (Guid.TryParse(base.Id, out var id))
            {
                return id;
            }

            return Guid.Empty;
        }
        set => base.Id = value.ToString();
    }

    /// <summary>
    /// The owner of the page.
    /// </summary>
    public Guid Owner { get; set; }

    /// <summary>
    /// The date when the page was created.
    /// </summary>
    public DateTime DateCreated { get; set; }

    /// <summary>
    /// The culture of the page.
    /// </summary>
    public string? Culture { get; set; }

    /// <summary>
    /// The alternate language content items of the page.
    /// </summary>
    [JsonIgnore]
    public List<ICultureSdkItem> AlternateLanguageContentItems { get; set; } = [];

    /// <summary>
    /// The URL of the page.
    /// </summary>
    /// <remarks>
    /// The sitefinity api sometimes returns an empty ViewUrl, this now falls back to RelativeUrlPath.
    /// </remarks>
    [JsonIgnore]
    public string Url =>
        !string.IsNullOrWhiteSpace(ViewUrl) ? ViewUrl : (RelativeUrlPath ?? string.Empty);

    /// <summary>
    /// The RelativeUrlPath of the page.
    /// </summary>
    public string? RelativeUrlPath { get; set; }
}
