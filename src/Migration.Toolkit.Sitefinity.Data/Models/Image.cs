namespace Migration.Toolkit.Data.Models;
/// <summary>
/// SdkItem model for Sitefinity images.
/// </summary>
public class Image : Media
{
    /// <summary>
    /// The Height of the image.
    /// </summary>
    public int Height { get; set; }
    /// <summary>
    /// The Width of the image.
    /// </summary>
    public int Width { get; set; }
}
