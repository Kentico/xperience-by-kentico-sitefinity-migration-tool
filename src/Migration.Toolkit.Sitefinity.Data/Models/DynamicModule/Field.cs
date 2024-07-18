namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Represents a field in the Sitefinity type definitions.
/// </summary>
public class Field
{
    /// <summary>
    /// The ID of the parent type
    /// </summary>
    public string? ParentTypeId { get; set; }

    /// <summary>
    /// The namespace of the field
    /// </summary>
    public string? FieldNamespace { get; set; }

    /// <summary>
    /// The name of the field
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The title of the field
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The type of the field
    /// </summary>
    public int FieldType { get; set; }

    /// <summary>
    /// The special type of the field
    /// </summary>
    public int SpecialType { get; set; }

    /// <summary>
    /// The media type of the field
    /// </summary>
    public string? MediaType { get; set; }

    /// <summary>
    /// The display name of the field type
    /// </summary>
    public string? FieldTypeDisplayName { get; set; }

    /// <summary>
    /// The minimum number range for the field
    /// </summary>
    public string? MinNumberRange { get; set; }

    /// <summary>
    /// The maximum number range for the field
    /// </summary>
    public string? MaxNumberRange { get; set; }

    /// <summary>
    /// Indicates if the field is required
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// The validation message for the length of the field
    /// </summary>
    public string? LengthValidationMessage { get; set; }

    /// <summary>
    /// Indicates if items can be created while selecting the field
    /// </summary>
    public bool CanCreateItemsWhileSelecting { get; set; }

    /// <summary>
    /// The widget type name of the field
    /// </summary>
    public string? WidgetTypeName { get; set; }

    /// <summary>
    /// The database type of the field
    /// </summary>
    public string? DBType { get; set; }

    /// <summary>
    /// The classification ID of the field
    /// </summary>
    public string? ClassificationId { get; set; }

    /// <summary>
    /// The database length of the field
    /// </summary>
    public string? DBLength { get; set; }

    /// <summary>
    /// The instructional choice of the field
    /// </summary>
    public string? InstructionalChoice { get; set; }

    /// <summary>
    /// The number unit of the field
    /// </summary>
    public string? NumberUnit { get; set; }

    /// <summary>
    /// The column name of the field
    /// </summary>
    public string? ColumnName { get; set; }

    /// <summary>
    /// The instructional text of the field
    /// </summary>
    public string? InstructionalText { get; set; }

    /// <summary>
    /// The allowed image extensions for the field
    /// </summary>
    public string? ImageExtensions { get; set; }

    /// <summary>
    /// The allowed video extensions for the field
    /// </summary>
    public string? VideoExtensions { get; set; }

    /// <summary>
    /// The allowed file extensions for the field
    /// </summary>
    public string? FileExtensions { get; set; }

    /// <summary>
    /// The default value of the field
    /// </summary>
    public string? DefaultValue { get; set; }

    /// <summary>
    /// The choices for the field
    /// </summary>
    public string? Choices { get; set; }

    /// <summary>
    /// The choice render type of the field
    /// </summary>
    public string? ChoiceRenderType { get; set; }

    /// <summary>
    /// The ID of the parent section
    /// </summary>
    public string? ParentSectionId { get; set; }

    /// <summary>
    /// The UI name of the field type
    /// </summary>
    public string? TypeUIName { get; set; }

    /// <summary>
    /// Indicates if the field is localizable
    /// </summary>
    public bool IsLocalizable { get; set; }

    /// <summary>
    /// The related data type of the field
    /// </summary>
    public string? RelatedDataType { get; set; }

    /// <summary>
    /// The ID of the field
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The last modified date of the field
    /// </summary>
    public DateTime? LastModified { get; set; }

    /// <summary>
    /// The number of decimal places for the field
    /// </summary>
    public int? DecimalPlacesCount { get; set; }
}
