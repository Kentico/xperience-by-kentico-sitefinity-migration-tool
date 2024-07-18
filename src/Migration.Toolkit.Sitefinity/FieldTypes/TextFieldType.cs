using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Core;

namespace Migration.Toolkit.Sitefinity.FieldTypes;
/// <summary>
/// Field type for Sitefinity Text field: "Telerik.Sitefinity.Web.UI.Fields.TextField"
/// </summary>
public class TextFieldType : FieldTypeBase, IFieldType
{
    public string SitefinityWidgetTypeName => "Telerik.Sitefinity.Web.UI.Fields.TextField";

    public override string? GetColumnSize(Field sitefinityField)
    {
        if (sitefinityField.FieldTypeDisplayName == null)
        {
            return sitefinityField.DBLength;
        }

        if (sitefinityField.FieldTypeDisplayName.Equals("Number"))
        {
            return "20";
        }

        return sitefinityField.DBLength;
    }

    public override string GetColumnType(Field sitefinityField)
    {
        if (sitefinityField.FieldTypeDisplayName == null)
        {
            return "text";
        }

        if (sitefinityField.FieldTypeDisplayName.Equals("LongText"))
        {
            return "longtext";
        }

        if (sitefinityField.FieldTypeDisplayName.Equals("Number"))
        {
            return "decimal";
        }

        return "text";
    }

    public override FormFieldSettings GetSettings(Field sitefinityField)
    {
        if (sitefinityField.FieldTypeDisplayName == null)
        {
            return new FormFieldSettings
            {
                ControlName = "Kentico.Administration.TextInput"
            };
        }

        if (sitefinityField.FieldTypeDisplayName.Equals("LongText"))
        {
            return new FormFieldSettings
            {
                ControlName = "Kentico.Administration.TextArea"
            };
        }

        if (sitefinityField.FieldTypeDisplayName.Equals("Number"))
        {
            return new FormFieldSettings
            {
                ControlName = "Kentico.Administration.DecimalNumberInput"
            };
        }

        return new FormFieldSettings
        {
            ControlName = "Kentico.Administration.TextInput"
        };
    }

    public override FormField HandleSpecialCase(FormField formField, Field sitefinityField)
    {
        if (sitefinityField.FieldTypeDisplayName == null)
        {
            return formField;
        }

        if (sitefinityField.FieldTypeDisplayName.Equals("Number"))
        {
            formField.Precision = sitefinityField.DecimalPlacesCount ?? 0;
        }

        return formField;
    }
}
