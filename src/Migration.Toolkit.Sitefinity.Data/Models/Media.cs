using Migration.Toolkit.Data.Models;

using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Tookit.Data.Models;
/// <summary>
/// SdkItem model for Sitefinity media. Can be used for images, videos or documents.
/// </summary>
public class Media : MediaDto, ISitefinityModel
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

    public DateTime LastModified { get; set; }

    public Guid? LastModifiedBy { get; set; }
    public Guid? CreatedBy { get; set; }
}
