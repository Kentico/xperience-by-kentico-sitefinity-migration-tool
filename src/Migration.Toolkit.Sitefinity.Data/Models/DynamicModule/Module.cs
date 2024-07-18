namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model used for dynamic module definitions from the deployment export.
/// </summary>
public class Module
{
    /// <summary>
    /// The name of the module.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The title of the module.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The description of the module.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The status of the module.
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// The types of the dynamic module.
    /// </summary>
    public List<DynamicModuleType>? Types { get; set; }

    /// <summary>
    /// The default backend definition name of the module.
    /// </summary>
    public object? DefaultBackendDefinitionName { get; set; }

    /// <summary>
    /// The page ID of the module.
    /// </summary>
    public string? PageId { get; set; }

    /// <summary>
    /// The URL name of the module.
    /// </summary>
    public string? UrlName { get; set; }

    /// <summary>
    /// The ID of the module.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// The owner of the module.
    /// </summary>
    public string? Owner { get; set; }
}
