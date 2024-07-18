namespace Migration.Toolkit.Data.Configuration;
/// <summary>
/// Configuration model for Sitefinity connections
/// </summary>
public class SitefinityDataConfiguration
{
    public required string SitefinityConnectionString { get; set; }
    public required string SitefinitySiteDomain { get; set; }
    public required string SitefinityRestApiUrl { get; set; }
    public string? SitefinityRestApiKey { get; set; }
    public required string SitefinityModuleDeploymentFolderPath { get; set; }
}
