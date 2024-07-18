namespace Migration.Toolkit.Sitefinity.Model;
public class ImageRelatedItem
{
    public Guid Identifier { get; set; }
    public string? Name { get; set; }
    public int Size { get; set; }
    public Dimensions? Dimensions { get; set; }
}

public class Dimensions
{
    public int Width { get; set; }
    public int Height { get; set; }
}
