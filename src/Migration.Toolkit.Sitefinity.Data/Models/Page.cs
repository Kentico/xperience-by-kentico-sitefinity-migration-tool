using System.Text.Json.Serialization;

using Progress.Sitefinity.RestSdk.Clients.Pages.Dto;

namespace Migration.Toolkit.Data.Models;
public class Page : PageNodeDto, ISitefinityModel, ICultureSdkItem
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
    public Guid Owner { get; set; }
    public DateTime DateCreated { get; set; }
    public string? Culture { get; set; }
    [JsonIgnore]
    public List<ICultureSdkItem> AlternateLanguageContentItems { get; set; } = [];
    [JsonIgnore]
    public string Url => ViewUrl;
}
