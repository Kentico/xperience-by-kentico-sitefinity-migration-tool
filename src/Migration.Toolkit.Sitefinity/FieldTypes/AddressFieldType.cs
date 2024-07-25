using System.Text.Json;

using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Core;

using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Toolkit.Sitefinity.FieldTypes;
/// <summary>
/// Field type for Sitefinity Address field: "Telerik.Sitefinity.Web.UI.Fields.AddressField".
/// </summary>
public class AddressFieldType : FieldTypeBase, IFieldType
{
    public string SitefinityWidgetTypeName => "Telerik.Sitefinity.Web.UI.Fields.AddressField";

    public override string GetColumnType(Field sitefinityField) => "longtext";

    public override FormFieldSettings GetSettings(Field sitefinityField) => new()
    {
        ControlName = "Kentico.Administration.TextArea"
    };

    public override object GetData(SdkItem sdkItem, string fieldName)
    {
        var address = sdkItem.GetValue<Address>(fieldName);

        return JsonSerializer.Serialize(address);
    }
}
