namespace Migration.Toolkit.Sitefinity.Model;

internal class Link
{
    public Guid Id { get; set; }
    public string? Href { get; set; }
    public string? Sfref { get; set; }
    public string? Text { get; set; }
    public string? Target { get; set; }
    public string? QueryParams { get; set; }
    public string? Anchor { get; set; }
    public string? Tooltip { get; set; }
    public string? Type { get; set; }
    public IEnumerable<string>? ClassList { get; set; }
    public Dictionary<string, string>? Attributes { get; set; }
}
