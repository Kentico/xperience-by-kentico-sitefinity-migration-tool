using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Toolkit.Data.Models;
public class Library : LibraryDto, ISitefinityModel
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
    public string? UrlName { get; set; }
    public string? BlobStorageProvider { get; set; }
    public DateTime LastModified { get; set; }
}
