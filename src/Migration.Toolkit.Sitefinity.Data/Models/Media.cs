using Migration.Toolkit.Data.Models;

using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Tookit.Data.Models;
/// <summary>
/// SdkItem model for Sitefinity media. Can be used for images, videos or documents.
/// </summary>
public class Media : MediaDto, ISitefinityModel
{
    /// <summary>
    /// The unique identifier of the media.
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
    /// The last modified date of the media.
    /// </summary>
    public DateTime LastModified { get; set; }

    /// <summary>
    /// The unique identifier of the user who last modified the media.
    /// </summary>
    public Guid? LastModifiedBy { get; set; }

    /// <summary>
    /// The unique identifier of the user who created the media.
    /// </summary>
    public Guid? CreatedBy { get; set; }
}
