
namespace Migration.Toolkit.Data.Models;
public partial class SystemCulture : ISitefinityModel
{
    public string? Culture { get; set; }
    public string? DisplayName { get; set; }
    public bool IsDefault { get; set; }
    public string? Key { get; set; }
    public string? ShortName { get; set; }
    public string? UICulture { get; set; }
    public Guid Id => Guid.NewGuid();
}
