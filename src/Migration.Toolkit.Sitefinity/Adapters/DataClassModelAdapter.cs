using CMS.Helpers;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Configuration;
using Migration.Toolkit.Sitefinity.Core.Factories;
using Migration.Toolkit.Sitefinity.Core.Helpers;
using Migration.Toolkit.Sitefinity.Model;

namespace Migration.Toolkit.Sitefinity.Adapters;
internal class DataClassModelAdapter(ILogger<DataClassModelAdapter> logger, SitefinityImportConfiguration configuration, IFieldTypeFactory fieldTypeFactory, ITypeHelper typeHelper) : UmtAdapterBaseWithDependencies<SitefinityType, DataClassDependencies>(logger)
{
    protected override IEnumerable<IUmtModel> AdaptInternal(SitefinityType source, DataClassDependencies dependenciesModel)
    {
        var websiteTypes = typeHelper.GetWebsiteTypes();

        bool isPageType = websiteTypes.Any(x => x.Id.Equals(source.Id)) || Array.Exists(Constants.ForcedWebsiteTypes, x => x.Equals(source.Name));
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
            ClassLastModified = source.LastModified ?? DateTime.Now,
            ClassHasUnmanagedDbSchema = false,
            ClassResourceGuid = null,
            ClassWebPageHasUrl = isPageType,
        };

        yield return dataClassModel;

        if (isPageType)
        {
            foreach (var channel in dependenciesModel.Channels)
            {
                var dataClassChannelModel = new ContentTypeChannelModel
                {
                    ContentTypeChannelChannelGuid = channel.Key,
                    ContentTypeChannelContentTypeGuid = dataClassModel.ClassGUID
                };

                yield return dataClassChannelModel;
            }
        }
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
            if (Constants.ExcludedFields.Contains(field.Name))
            {
                continue;
            }

            var fieldType = fieldTypeFactory.CreateFieldType(field.WidgetTypeName);

            var formField = new FormField
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
            };

            fieldType.HandleSpecialCase(formField, field);

            formFields.Add(formField);
        }

        return formFields;
    }

    private FormFieldProperties MapProperties(Field field) => new()
    {
        FieldCaption = field.Title
    };
}
