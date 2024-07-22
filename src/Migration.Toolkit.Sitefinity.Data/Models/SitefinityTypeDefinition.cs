namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model used to define the types when calling the content provider.
/// </summary>
public class SitefinityTypeDefinition
{
    /// <summary>
    /// The namespace of the Sitefinity type.
    /// </summary>
    public required string SitefinityTypeNameSpace { get; set; }

    /// <summary>
    /// The name of the Sitefinity type.
    /// </summary>
    public required string SitefinityTypeName { get; set; }

    /// <summary>
    /// The GUID of the data class.
    /// </summary>
    public required Guid DataClassGuid { get; set; }
}
