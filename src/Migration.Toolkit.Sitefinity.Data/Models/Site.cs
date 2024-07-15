namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model of Sitefinity site
/// </summary>
public partial class Site : ISitefinityModel
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? LiveUrl { get; set; }

    public IEnumerable<SystemCulture>? Cultures { get; set; }
}
