namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Abstract class for Static or Dynamic Module Sitefinity types.
/// </summary>
public abstract class SitefinityType : ISitefinityModel
{
    /// <summary>
    /// The display name of the Sitefinity type.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// The name of the Sitefinity type.
    /// </summary>
    public abstract string? Name { get; }

    /// <summary>
    /// The ID of the Sitefinity type.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The list of fields associated with the Sitefinity type.
    /// </summary>
    public List<Field>? Fields { get; set; }

    /// <summary>
    /// The last modified date of the Sitefinity type.
    /// </summary>
    public DateTime? LastModified { get; set; }

    /// <summary>
    /// The class namespace of the Sitefinity type.
    /// </summary>
    public abstract string? ClassNamespace { get; }

    /// <summary>
    /// The ID of the parent module type.
    /// </summary>
    public Guid? ParentModuleTypeId { get; set; }
}
