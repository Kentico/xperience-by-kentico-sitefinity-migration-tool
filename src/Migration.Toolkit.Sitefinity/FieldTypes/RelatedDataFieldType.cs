using System.Text.Json;

using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Core;
using Migration.Toolkit.Sitefinity.Model;

using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Toolkit.Sitefinity.FieldTypes;
/// <summary>
/// Field type for Sitefinity RelatedData field: "Telerik.Sitefinity.Web.UI.Fields.RelatedDataField"
/// </summary>
public class RelatedDataFieldType(ITypeProvider typeProvider) : FieldTypeBase, IFieldType
{
    private IEnumerable<SitefinityType>? sitefinityTypes;

    public string SitefinityWidgetTypeName => "Telerik.Sitefinity.Web.UI.Fields.RelatedDataField";

    public override string GetColumnType(Field sitefinityField)
    {
        if (sitefinityField.RelatedDataType == null)
        {
            return "contentitemreference";
        }

        if (sitefinityField.RelatedDataType.Equals("Telerik.Sitefinity.Pages.Model.PageNode"))
        {
            return "webpages";
        }

        return "contentitemreference";
    }
    public override FormFieldSettings GetSettings(Field sitefinityField)
    {
        sitefinityTypes ??= typeProvider.GetAllTypes();

        if (sitefinityField.RelatedDataType != null && sitefinityField.RelatedDataType.Equals("Telerik.Sitefinity.Pages.Model.PageNode"))
        {
            return new FormFieldSettings
            {
                ControlName = "Kentico.Administration.WebPageSelector",
                CustomProperties = new Dictionary<string, object?>
                {
                    { "TreePath", "/" },
                }
            };
        }


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
    public override object GetData(SdkItem sdkItem, string fieldName)
    {
        var relatedData = sdkItem.GetValue<IEnumerable<RelatedItem>>(fieldName);

        var contentRelatedItems = new List<ContentRelatedItem>();

        foreach (var item in relatedData)
        {
            if (Guid.TryParse(item.Id, out var result))
            {
                contentRelatedItems.Add(new ContentRelatedItem
                {
                    Identifier = result
                });
            }
        }

        return JsonSerializer.Serialize(contentRelatedItems);
    }
}
