using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core;

namespace Migration.Toolkit.Sitefinity.FieldTypes;
public class RelatedDataFieldType : IFieldType
{
    private readonly IEnumerable<SitefinityType> sitefinityTypes;
    public string SitefinityWidgetTypeName => "Telerik.Sitefinity.Web.UI.Fields.RelatedDataField";

    public RelatedDataFieldType(ITypeProvider typeProvider) => sitefinityTypes = typeProvider.GetAllTypes();

    public string? GetColumnSize(Field sitefinityField) => sitefinityField.DBLength;
    public string GetColumnType(Field sitefinityField) => "contentitemreference";
    public FormFieldSettings GetSettings(Field sitefinityField)
    {
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
