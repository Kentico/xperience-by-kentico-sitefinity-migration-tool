namespace Migration.Tookit.Data.Configuration;
/// <summary>
/// Configuration model for Sitefinity connections
/// </summary>
public class SitefinityToolkitConfiguration
{
    public required string SitefinityConnectionString { get; set; }
    public required string SitefinityRestApiUrl { get; set; }
    public string? SitefinityRestApiKey { get; set; }
}
