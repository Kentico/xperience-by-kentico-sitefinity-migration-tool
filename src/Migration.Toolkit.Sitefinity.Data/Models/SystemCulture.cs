
namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model used for a site's available cultures.
/// </summary>
public partial class SystemCulture : ISitefinityModel
{
    /// <summary>
    /// The culture of the system.
    /// </summary>
    public string? Culture { get; set; }

    /// <summary>
    /// The display name of the system culture.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Indicates whether the system culture is the default culture.
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// The key of the system culture.
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// The short name of the system culture.
    /// </summary>
    public string? ShortName { get; set; }

    /// <summary>
    /// The UI culture of the system.
    /// </summary>
    public string? UICulture { get; set; }

    public Guid Id => Guid.NewGuid();
}
