using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Toolkit.Data.Models;
/// <summary>
/// SdkItem model for Sitefinity libraries.
/// </summary>
public class Library : LibraryDto, ISitefinityModel
{
    /// <summary>
    /// Unique identifier of the library.
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
    /// URL name of the library.
    /// </summary>
    public string? UrlName { get; set; }

    /// <summary>
    /// Blob storage provider of the library.
    /// </summary>
    public string? BlobStorageProvider { get; set; }

    /// <summary>
    /// Last modified date of the library.
    /// </summary>
    public DateTime LastModified { get; set; }
}
