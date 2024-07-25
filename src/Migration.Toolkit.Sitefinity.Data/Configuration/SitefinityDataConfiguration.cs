namespace Migration.Toolkit.Data.Configuration;
/// <summary>
/// Configuration model for Sitefinity connections.
/// </summary>
public class SitefinityDataConfiguration
{
    /// <summary>
    /// Sitefinity connection string used in Entity Framework.
    /// </summary>
    public required string SitefinityConnectionString { get; set; }

    /// <summary>
    /// Sitefinity site url used to create absolute urls.
    /// </summary>
    public required string SitefinitySiteUrl { get; set; }

    /// <summary>
    /// Sitefinity site url used to build the rest api url.
    /// </summary>
    public required string SitefinityRestApiUrl { get; set; }

    /// <summary>
    /// Sitefinity rest api key used to authenticate with the rest api. Optional.
    /// </summary>
    public string? SitefinityRestApiKey { get; set; }

    /// <summary>
    /// Sitefinity module deployment folder path used when importing type definitions.
    /// </summary>
    public required string SitefinityModuleDeploymentFolderPath { get; set; }
}
