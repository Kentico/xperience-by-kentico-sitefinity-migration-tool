namespace Migration.Tookit.Data.Models;
/// <summary>
/// SdkItem model for Sitefinity images.
/// </summary>
public class Image : Media
{
    public int Height { get; set; }
    public int Width { get; set; }
}
