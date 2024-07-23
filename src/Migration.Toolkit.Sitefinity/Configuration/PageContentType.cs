using System.Text.Json.Serialization;

namespace Migration.Toolkit.Sitefinity.Configuration
{
    /// <summary>
    /// Configuration for which content types should be Website types and the location of the child pages in XbyK.
    /// </summary>
    public class PageContentType
    {
        /// <summary>
        /// Name of Sitefinity content type. OOB or custom dynamic module.
        /// </summary>
        public required string TypeName { get; set; }
        /// <summary>
        /// Root path where the child pages will be created in XbyK.
        /// </summary>
        public required string PageRootPath { get; set; }
        public string? ItemUrlName { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required PageTemplateType PageTemplateType { get; set; }
    }

    public enum PageTemplateType
    {
        Listing,
        Detail
    }
}
