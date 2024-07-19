namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model for static Sitefinity types. For example, NewsItem, BlogPost, ContentBlock, etc.
/// </summary>
public class StaticSitefinityType : SitefinityType
{
    /// <summary>
    /// The class name.
    /// </summary>
    public string? ClassName { get; set; }

    /// <summary>
    /// The name of the type.
    /// </summary>
    public override string? Name => ClassName;

    /// <summary>
    /// The namespace.
    /// </summary>
    public string? Namespace { get; set; }

    /// <summary>
    /// The namespace of the class.
    /// </summary>
    public override string? ClassNamespace => Namespace;
}
