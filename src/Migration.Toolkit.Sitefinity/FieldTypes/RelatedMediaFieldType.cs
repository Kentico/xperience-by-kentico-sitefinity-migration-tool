using System.Text.Json;

using CMS.DataEngine;

using Kentico.Xperience.UMT.Model;

using Microsoft.Extensions.Logging;

using Migration.Toolkit.Data.Core.Providers;
using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Abstractions;
using Migration.Toolkit.Sitefinity.Core;
using Migration.Toolkit.Sitefinity.Data;
using Migration.Toolkit.Sitefinity.Model;

using Progress.Sitefinity.RestSdk.Dto;

namespace Migration.Toolkit.Sitefinity.FieldTypes;
/// <summary>
/// Field type for Sitefinity Related Media field: "Telerik.Sitefinity.Web.UI.Fields.RelatedMediaField".
/// </summary>
public class RelatedMediaFieldType(ITypeProvider typeProvider, ILogger<RelatedMediaFieldType> logger) : FieldTypeBase, IFieldType
{
    private IEnumerable<SitefinityType>? sitefinityTypes;

    public string SitefinityWidgetTypeName => "Telerik.Sitefinity.Web.UI.Fields.RelatedMediaField";

    public override string GetColumnType(Field sitefinityField) => FieldDataType.ContentItemReference;

    public override FormFieldSettings GetSettings(Field sitefinityField)
    {
        sitefinityTypes ??= typeProvider.GetAllTypes();

        // Get the media content types dynamically
        var mediaContentTypes = sitefinityTypes.Where(x =>
            x.ClassNamespace == SitefinityMigrationConstants.MigratedFileTypeDefaultNamespace &&
            (x.Name == SitefinityMigrationConstants.MigratedImageDefaultTypeName
             || x.Name == SitefinityMigrationConstants.MigratedVideoDefaultTypeName
             || x.Name == SitefinityMigrationConstants.MigratedDownloadDefaultTypeName)).ToArray();

        string maxItems = "100"; // Default maximum
        if (!string.IsNullOrEmpty(sitefinityField.MaxNumberRange) && !sitefinityField.MaxNumberRange.Equals("0"))
        {
            maxItems = sitefinityField.MaxNumberRange;
        }

        return new FormFieldSettings
        {
            ControlName = "Kentico.Administration.ContentItemSelector",
            CustomProperties = new Dictionary<string, object?>
            {
                { "SelectionType", "contentTypes" },
                { "AllowedContentItemTypeIdentifiers", $"[\"{string.Join("\", \"", mediaContentTypes.Select(x => x.Id))}\"]" },
                { "MaximumItems", maxItems },
                { "MinimumItems", sitefinityField.IsRequired ? "1" : "0" }
            }
        };
    }

    public override object GetData(SdkItem sdkItem, string fieldName)
    {
        IEnumerable<ImageDto>? relatedData = null;

        try
        {
            if (sdkItem.TryGetValue<ImageDto>(fieldName, out var singleItem) && singleItem != null)
            {
                relatedData = [singleItem];
            }
        }
        catch
        {
            // Intentionally left blank to ignore exceptions when attempting to get single item.
        }

        // Try to get as single item and wrap in enumerable if enumerable attempt failed
        if (relatedData == null)
        {
            try
            {
                if (sdkItem.TryGetValue<IEnumerable<ImageDto>>(fieldName, out var enumerable))
                {
                    relatedData = enumerable;
                }
            }
            catch
            {
                // Intentionally left blank to ignore exceptions when attempting to get single item.
            }
        }

        if (relatedData == null || !relatedData.Any())
        {
            logger.LogDebug("No related media data found for field {FieldName} in content item {ContentItemId}", fieldName, sdkItem.Id);
            return JsonSerializer.Serialize(new List<ContentRelatedItem>());
        }

        var contentRelatedItems = new List<ContentRelatedItem>();

        foreach (var item in relatedData)
        {
            if (Guid.TryParse(item.Id, out var mediaItemGuid))
            {
                contentRelatedItems.Add(new ContentRelatedItem
                {
                    Identifier = mediaItemGuid
                });
            }
        }

        return JsonSerializer.Serialize(contentRelatedItems);
    }

    public override FormField HandleSpecialCase(FormField formField, Field sitefinityField)
    {
        // Ensure the field is properly configured as a content item reference field
        formField.AllowEmpty = !sitefinityField.IsRequired;
        formField.ColumnType = FieldDataType.ContentItemReference;
        formField.Properties ??= new FormFieldProperties();

        if (string.IsNullOrEmpty(formField.Properties.FieldCaption))
        {
            formField.Properties.FieldCaption = string.IsNullOrEmpty(sitefinityField.Title) ? sitefinityField.Name : sitefinityField.Title;
        }

        return formField;
    }
}
