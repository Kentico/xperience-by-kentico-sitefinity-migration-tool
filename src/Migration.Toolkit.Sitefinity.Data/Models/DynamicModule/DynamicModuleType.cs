namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model used for dynamic module type definitions from the deployment export.
/// </summary>
public class DynamicModuleType : SitefinityType
{
    public string? ParentModuleId { get; set; }
    public string? TypeNamespace { get; set; }
    public string? TypeName { get; set; }
    public string? MainShortTextFieldName { get; set; }
    public bool IsSelfReferencing { get; set; }
    public bool CheckFieldPermissions { get; set; }
    public string? PageId { get; set; }
    public IEnumerable<Section>? Sections { get; set; }
    public override string? Name => TypeName;
    public override string? ClassNamespace => TypeNamespace;
}
