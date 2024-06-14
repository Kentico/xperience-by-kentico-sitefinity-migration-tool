using CMS.Helpers;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Configuration;
using Migration.Toolkit.Sitefinity.Core.Factories;

namespace Migration.Toolkit.Sitefinity.Adapters;
internal class DataClassModelAdapter(ILogger<DataClassModelAdapter> logger, SitefinityImportConfiguration configuration, IFieldTypeFactory fieldTypeFactory) : UmtAdapterBase<SitefinityType, DataClassModel>(logger)
{
    protected override DataClassModel AdaptInternal(SitefinityType source)
    {
        bool isPageType = configuration.PageContentTypes != null && configuration.PageContentTypes.Any(x => x.Name.Equals(source.Name));
        var dataClassModel = new DataClassModel
        {
            ClassDisplayName = source.DisplayName,
            ClassGUID = source.Id,
            ClassName = $"{configuration.SitefinityCodeNamePrefix}.{source.Name}",
            ClassShortName = $"{configuration.SitefinityCodeNamePrefix}.{source.Name}",
            ClassTableName = $"{configuration.SitefinityCodeNamePrefix}_{source.Name}",
            ClassType = "Content",
            Fields = MapFields(source.Fields),
            ClassContentTypeType = isPageType
            ? "Website"
            : "Reusable",
            ClassLastModified = source.LastModified,
            ClassHasUnmanagedDbSchema = false,
            ClassResourceGuid = null,
            ClassWebPageHasUrl = isPageType,
        };

        return dataClassModel;
    }

    private List<FormField> MapFields(IEnumerable<Field>? fields)
    {
        var formFields = new List<FormField>();

        if (fields == null)
        {
            return formFields;
        }

        foreach (var field in fields)
        {
            var fieldType = fieldTypeFactory.CreateFieldType(field.WidgetTypeName);

            formFields.Add(new FormField
            {
                AllowEmpty = !field.IsRequired,
                Column = !string.IsNullOrEmpty(field.ColumnName) ? field.ColumnName : field.Name,
                ColumnType = fieldType.GetColumnType(field),
                Enabled = true,
                Guid = field.Id,
                Visible = true,
                Properties = MapProperties(field),
                Settings = fieldType.GetSettings(field),
                ColumnSize = ValidationHelper.GetInteger(fieldType.GetColumnSize(field), 255),
            });
        }

        return formFields;
    }

    private FormFieldProperties MapProperties(Field field) => new()
    {
        FieldCaption = field.Title
    };
}
