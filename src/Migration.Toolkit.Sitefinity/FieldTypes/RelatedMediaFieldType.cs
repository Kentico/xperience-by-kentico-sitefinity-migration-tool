using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core;

namespace Migration.Toolkit.Sitefinity.FieldTypes;
public class RelatedMediaFieldType : IFieldType
{
    public string SitefinityWidgetTypeName => "Telerik.Sitefinity.Web.UI.Fields.RelatedMediaField";

    public string? GetColumnSize(Field sitefinityField) => sitefinityField.DBLength;
    public string GetColumnType(Field sitefinityField) => "assets";
    public FormFieldSettings GetSettings(Field sitefinityField) => new()
    {
        ControlName = "Kentico.Administration.AssetSelector",
        CustomProperties = new()
        {
            { "AllowedExtensions", !string.IsNullOrEmpty(sitefinityField.FileExtensions) ? sitefinityField.FileExtensions.Replace(".", "").Replace(',', ';') : "_INHERITED_" },
            { "MaximumAssets", !string.IsNullOrEmpty(sitefinityField.MaxNumberRange) && sitefinityField.MaxNumberRange.Equals("0") ? "100" : sitefinityField.MaxNumberRange }
        }
    };
}
