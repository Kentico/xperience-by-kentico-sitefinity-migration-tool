namespace Migration.Toolkit.Data.Models;

/// <summary>
/// SdkItem model for Sitefinity videos.
/// </summary>
public class Video : Media
{
    /// <summary>
    /// The duration of the video in seconds.
    /// </summary>
    public int Duration { get; set; }
    /// <summary>
    /// The width of the video.
    /// </summary>
    public int Width { get; set; }
    /// <summary>
    /// The height of the video.
    /// </summary>
    public int Height { get; set; }
}
