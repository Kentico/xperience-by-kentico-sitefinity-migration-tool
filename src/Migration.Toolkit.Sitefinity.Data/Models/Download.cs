namespace Migration.Toolkit.Data.Models;

/// <summary>
/// SdkItem model for Sitefinity downloads/documents.
/// </summary>
public class Download : Media
{
    /// <summary>
    /// The number of pages in the document (for PDFs and similar).
    /// </summary>
    public int? PageCount { get; set; }
}
