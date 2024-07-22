using System.Text.Json.Serialization;

using Progress.Sitefinity.RestSdk.Dto.Content;

namespace Migration.Toolkit.Data.Models;
/// <summary>
/// SdkItem model for Sitefinity static or dynamic module content items.
/// </summary>
public class ContentItem : ContentWithParentDto, ISitefinityModel, ICultureSdkItem
{
    /// <summary>
    /// Unique identifier of the content item.
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
    /// Data class GUID of the content item.
    /// </summary>
    public Guid DataClassGuid { get; set; }

    /// <summary>
    /// Owner of the content item.
    /// </summary>
    public Guid Owner { get; set; }

    /// <summary>
    /// Last modified date of the content item.
    /// </summary>
    public DateTime LastModified { get; set; }

    /// <summary>
    /// Change type of the content item.
    /// </summary>
    public string? ChangeType { get; set; }

    /// <summary>
    /// Culture of the content item.
    /// </summary>
    public string? Culture { get; set; }

    /// <summary>
    /// Type name of the content item.
    /// </summary>
    [JsonIgnore]
    public string? TypeName { get; set; }

    /// <summary>
    /// Alternate language content items of the content item.
    /// </summary>
    [JsonIgnore]
    public List<ICultureSdkItem> AlternateLanguageContentItems { get; set; } = [];

    /// <summary>
    /// URL of the content item.
    /// </summary>
    [JsonIgnore]
    public string Url => ItemDefaultUrl;
}
