namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model of Sitefinity site
/// </summary>
public partial class Site : ISitefinityModel
{
    /// <summary>
    /// The unique identifier of the site
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the site
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The live URL of the site
    /// </summary>
    public string? LiveUrl { get; set; }

    /// <summary>
    /// The staging URL of the site
    /// </summary>
    public string? StagingUrl { get; set; }

    /// <summary>
    /// The system cultures supported by the site
    /// </summary>
    public required IEnumerable<SystemCulture> SystemCultures { get; set; }
}
