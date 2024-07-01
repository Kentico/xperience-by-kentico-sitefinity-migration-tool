using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Sitefinity.Abstractions;
/// <summary>
/// Base class for field types. Provides default implementations for field type properties.
/// </summary>
public class FieldTypeBase
{
    public virtual string? GetColumnSize(Field sitefinityField) => sitefinityField.DBLength;
    public virtual string GetColumnType(Field sitefinityField) => "text";
    public virtual FormFieldSettings GetSettings(Field sitefinityField) => new()
    {
        ControlName = "Kentico.Administration.TextInput"
    };
    public virtual FormField HandleSpecialCase(FormField formField, Field sitefinityField) => formField;
}
