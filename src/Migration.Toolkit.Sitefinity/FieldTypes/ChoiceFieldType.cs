using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Core;

using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Toolkit.Sitefinity.FieldTypes;
/// <summary>
/// Field type for Sitefinity Choice field: "Telerik.Sitefinity.Web.UI.Fields.ChoiceField".
/// </summary>
public class ChoiceFieldType : FieldTypeBase, IFieldType
{
    public string SitefinityWidgetTypeName => "Telerik.Sitefinity.Web.UI.Fields.ChoiceField";

    public override string GetColumnType(Field sitefinityField) => "boolean";

    public override FormFieldSettings GetSettings(Field sitefinityField) => new()
    {
        ControlName = "Kentico.Administration.Checkbox"
    };
    public override object GetData(SdkItem sdkItem, string fieldName) => sdkItem.GetValue<bool>(fieldName);
}
