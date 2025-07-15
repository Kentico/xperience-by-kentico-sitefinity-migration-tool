namespace Migration.Toolkit.Sitefinity.Configuration
{
    /// <summary>
    /// Configuration for importing objects and content into XbyK site from Sitefinity.
    /// </summary>
    public class SitefinityImportConfiguration
    {
        /// <summary>
        /// Prefix used in XbyK code names.
        /// </summary>
        public required string SitefinityCodeNamePrefix { get; set; }
        /// <summary>
        /// Used to translate Sitefinity auto routing pages into physical child pages in XbyK.
        /// </summary>
        public IEnumerable<PageContentType>? PageContentTypes { get; set; }
        /// <summary>
        /// Kentico workspace name used when importing content items.
        /// </summary>
        public required string KenticoWorkspaceName { get; set; }
    }
}
