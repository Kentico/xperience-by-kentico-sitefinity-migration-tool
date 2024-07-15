using Migration.Toolkit.Data.Models;

using Progress.Sitefinity.RestSdk.Dto.Content;

namespace Migration.Toolkit.Data.Models;
public class ContentItem : ContentWithParentDto, ISitefinityModel
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
}
