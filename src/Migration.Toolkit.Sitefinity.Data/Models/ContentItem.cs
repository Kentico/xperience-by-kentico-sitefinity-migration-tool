using System.Text.Json.Serialization;

using Progress.Sitefinity.RestSdk.Dto.Content;

namespace Migration.Toolkit.Data.Models;
public class ContentItem : ContentWithParentDto, ISitefinityModel, ICultureSdkItem
{
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

    public Guid DataClassGuid { get; set; }
    public Guid Owner { get; set; }
    public DateTime LastModified { get; set; }
    public string? ChangeType { get; set; }
    public string? Culture { get; set; }
    [JsonIgnore]
    public string? TypeName { get; set; }
    [JsonIgnore]
    public List<ICultureSdkItem> AlternateLanguageContentItems { get; set; } = [];
    [JsonIgnore]
    public string Url => ItemDefaultUrl;
}
