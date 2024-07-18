using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Core;

using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Toolkit.Sitefinity.FieldTypes;
/// <summary>
/// Field type for Sitefinity Date field: "Telerik.Sitefinity.Web.UI.Fields.DateField"
/// </summary>
public class DateFieldType : FieldTypeBase, IFieldType
{
    public string SitefinityWidgetTypeName => "Telerik.Sitefinity.Web.UI.Fields.DateField";

    public override string GetColumnType(Field sitefinityField) => "datetime";
    public override FormFieldSettings GetSettings(Field sitefinityField) => new()
    {
        ControlName = "Kentico.Administration.DateTimeInput"
    };
    public override object GetData(SdkItem sdkItem, string fieldName) => sdkItem.GetValue<DateTime>(fieldName);
}
