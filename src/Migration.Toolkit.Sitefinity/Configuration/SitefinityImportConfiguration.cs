﻿namespace Migration.Toolkit.Sitefinity.Configuration
{
    public class SitefinityImportConfiguration
    {
        public required string SitefinityCodeNamePrefix { get; set; }
        public IEnumerable<PageContentType>? PageContentTypes { get; set; }
    }
}