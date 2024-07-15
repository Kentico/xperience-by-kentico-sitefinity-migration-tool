using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Core;

namespace Migration.Toolkit.Sitefinity.FieldTypes;
/// <summary>
/// Field type for Sitefinity RelatedData field: "Telerik.Sitefinity.Web.UI.Fields.RelatedDataField"
/// </summary>
public class RelatedDataFieldType(ITypeProvider typeProvider) : FieldTypeBase, IFieldType
{
    private IEnumerable<SitefinityType>? sitefinityTypes;

    public string SitefinityWidgetTypeName => "Telerik.Sitefinity.Web.UI.Fields.RelatedDataField";

    public override string GetColumnType(Field sitefinityField) => "contentitemreference";
    public override FormFieldSettings GetSettings(Field sitefinityField)
    {
        sitefinityTypes ??= typeProvider.GetAllTypes();

        var allowedType = sitefinityTypes.FirstOrDefault(x => $"{x.ClassNamespace}.{x.Name}".Equals(sitefinityField.RelatedDataType));

        return new FormFieldSettings
        {
            ControlName = "Kentico.Administration.ContentItemSelector",
            CustomProperties = new Dictionary<string, object?>
            {
                { "SelectionType", "contentTypes" },
                { "AllowedContentItemTypeIdentifiers", $"[\"{allowedType?.Id}\"]" }
            }
        };
    }

}
