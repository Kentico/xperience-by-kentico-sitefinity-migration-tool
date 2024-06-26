using Migration.Toolkit.Data.Models;

using Progress.Sitefinity.RestSdk.Clients.Pages.Dto;

namespace Migration.Tookit.Data.Models;
public class Page : PageNodeDto, ISitefinityModel
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
}
