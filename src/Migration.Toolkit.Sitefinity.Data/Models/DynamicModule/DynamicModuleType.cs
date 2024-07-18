namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model used for dynamic module type definitions from the deployment export.
/// </summary>
public class DynamicModuleType : SitefinityType
{
    /// <summary>
    /// Parent module id.
    /// </summary>
    public string? ParentModuleId { get; set; }

    /// <summary>
    /// Namespace of the type.
    /// </summary>
    public string? TypeNamespace { get; set; }

    /// <summary>
    /// Code name of the type.
    /// </summary>
    public string? TypeName { get; set; }

    /// <summary>
    /// Main short text field name.
    /// </summary>
    public string? MainShortTextFieldName { get; set; }

    /// <summary>
    /// Indicates if the type is self-referencing.
    /// </summary>
    public bool IsSelfReferencing { get; set; }

    /// <summary>
    /// Indicates if field permissions should be checked.
    /// </summary>
    public bool CheckFieldPermissions { get; set; }

    /// <summary>
    /// Page id.
    /// </summary>
    public string? PageId { get; set; }

    /// <summary>
    /// List of sections.
    /// </summary>
    public IEnumerable<Section>? Sections { get; set; }

    /// <summary>
    /// Name of the type.
    /// </summary>
    public override string? Name => TypeName;

    /// <summary>
    /// Namespace of the class.
    /// </summary>
    public override string? ClassNamespace => TypeNamespace;
}
