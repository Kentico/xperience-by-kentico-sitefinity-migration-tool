namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model for sections in a type definition.
/// </summary>
public class Section
{
    /// <summary>
    /// The name of the section.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The ordinal value of the section.
    /// </summary>
    public int Ordinal { get; set; }

    /// <summary>
    /// The title of the section.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// A value indicating whether the section is expandable.
    /// </summary>
    public bool IsExpandable { get; set; }

    /// <summary>
    /// A value indicating whether the section is expanded by default.
    /// </summary>
    public bool IsExpandedByDefault { get; set; }

    /// <summary>
    /// The parent type ID of the section.
    /// </summary>
    public string? ParentTypeId { get; set; }

    /// <summary>
    /// The ID of the section.
    /// </summary>
    public string? Id { get; set; }
}
