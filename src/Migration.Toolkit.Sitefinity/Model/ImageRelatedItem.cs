namespace Migration.Toolkit.Sitefinity.Model;

internal class ImageRelatedItem
{
    public Guid Identifier { get; set; }
    public string? Name { get; set; }
    public int Size { get; set; }
    public Dimensions? Dimensions { get; set; }
}

internal class Dimensions
{
    public int Width { get; set; }
    public int Height { get; set; }
}
