namespace Migration.Tookit.Data.Configuration
{
    public class SitefinityToolkitConfiguration
    {
        public required string SitefinityConnectionString { get; set; }
        public required string SitefinityRestApiUrl { get; set; }
        public string? SitefinityRestApiKey { get; set; }
    }
}
