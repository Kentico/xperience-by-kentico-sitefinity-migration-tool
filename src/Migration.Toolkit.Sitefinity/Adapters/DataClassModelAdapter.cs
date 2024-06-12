using CMS.Helpers;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Tookit.Data.Models;
using Migration.Tookit.Sitefinity.Abstractions;
using Migration.Tookit.Sitefinity.Configuration;
using Migration.Tookit.Sitefinity.Helpers;

namespace Migration.Tookit.Sitefinity.Mappers
{
    internal class DataClassModelAdapter(ILogger<DataClassModelAdapter> logger, SitefinityImportConfiguration configuration) : UmtAdapterBase<SitefinityType, DataClassModel>(logger)
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
                formFields.Add(new FormField
                {
                    AllowEmpty = !field.IsRequired,
                    Column = !string.IsNullOrEmpty(field.ColumnName) ? field.ColumnName : field.Name,
                    ColumnType = FieldHelper.MapColumnType(field.DBType),
                    Enabled = true,
                    Guid = field.Id,
                    Visible = true,
                    Properties = MapProperties(field),
                    Settings = MapSettings(field),
                    ColumnSize = ValidationHelper.GetInteger(!string.IsNullOrEmpty(field.DBLength) ? field.DBLength : "200", 200),
                });
            }

            return formFields;
        }

        private FormFieldProperties MapProperties(Field field) => new()
        {
            FieldCaption = field.Title
        };

        private FormFieldSettings MapSettings(Field field) => new()
        {
            ControlName = FieldHelper.MapControlType(field.WidgetTypeName),
        };
    }
}
