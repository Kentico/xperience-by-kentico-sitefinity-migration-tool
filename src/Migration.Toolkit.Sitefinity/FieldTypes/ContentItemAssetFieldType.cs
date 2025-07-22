using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Core;

namespace Migration.Toolkit.Sitefinity.FieldTypes;

/// <summary>
/// Field type for Kentico Content Item Asset field: "Kentico.Administration.ContentItemAssetUploader".
/// </summary>
public class ContentItemAssetFieldType : FieldTypeBase, IFieldType
{
    public string SitefinityWidgetTypeName => "Kentico.Administration.ContentItemAssetUploader";

    public override string GetColumnType(Field sitefinityField) => "contentitemasset";

    public override FormFieldSettings GetSettings(Field sitefinityField) => new()
    {
        ControlName = "Kentico.Administration.ContentItemAssetUploader",
        CustomProperties = new()
        {
            { "AllowedExtensions", !string.IsNullOrEmpty(sitefinityField.FileExtensions) ? sitefinityField.FileExtensions.Replace(".", "").Replace(',', ';') : "_INHERITED_" },
            { "IsFormatConversionEnabled", "False" }
        }
    };

    public override FormField HandleSpecialCase(FormField formField, Field sitefinityField)
    {
        // Ensure the field is properly configured as a content item asset field
        formField.AllowEmpty = !sitefinityField.IsRequired;
        formField.ColumnType = "contentitemasset";
        formField.Properties ??= new FormFieldProperties();

        if (string.IsNullOrEmpty(formField.Properties.FieldCaption))
        {
            formField.Properties.FieldCaption = string.IsNullOrEmpty(sitefinityField.Title) ? sitefinityField.Name : sitefinityField.Title;
        }

        return formField;
    }
}
