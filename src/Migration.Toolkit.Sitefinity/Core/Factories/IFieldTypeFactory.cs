namespace Migration.Toolkit.Sitefinity.Core.Factories;
/// <summary>
/// Factory for creating field types to map Sitefinity fields to XbyK fields
/// </summary>
public interface IFieldTypeFactory
{
    /// <summary>
    /// Creates a field type based on the Sitefinity field type name
    /// </summary>
    /// <param name="fieldType">Field type name</param>
    /// <returns>FieldType class based on field type name</returns>
    public IFieldType CreateFieldType(string? fieldType);
}
