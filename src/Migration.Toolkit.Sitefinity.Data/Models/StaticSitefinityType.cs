namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model for static Sitefinity types. For example, NewsItem, BlogPost, ContentBlock, etc.
/// </summary>
public class StaticSitefinityType : SitefinityType
{
    public string? ClassName { get; set; }
    public override string? Name => ClassName;
    public string? Namespace { get; set; }
    public override string? ClassNamespace => Namespace;
}
