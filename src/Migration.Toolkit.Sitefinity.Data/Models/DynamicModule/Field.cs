namespace Migration.Tookit.Data.Models
{
    public class Field
    {
        public string? ParentTypeId { get; set; }
        public string? FieldNamespace { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public int FieldType { get; set; }
        public int SpecialType { get; set; }
        public string? MediaType { get; set; }
        public string? FieldTypeDisplayName { get; set; }
        public bool IsRequired { get; set; }
        public string? LengthValidationMessage { get; set; }
        public bool CanCreateItemsWhileSelecting { get; set; }
        public string? WidgetTypeName { get; set; }
        public string? DBType { get; set; }
        public string? ClassificationId { get; set; }
        public string? DBLength { get; set; }
        public string? InstructionalChoice { get; set; }
        public string? NumberUnit { get; set; }
        public string? ColumnName { get; set; }
        public string? InstructionalText { get; set; }
        public string? ImageExtensions { get; set; }
        public string? VideoExtensions { get; set; }
        public string? FileExtensions { get; set; }
        public string? DefaultValue { get; set; }
        public string? Choices { get; set; }
        public string? ChoiceRenderType { get; set; }
        public string? ParentSectionId { get; set; }
        public string? TypeUIName { get; set; }
        public bool IsLocalizable { get; set; }
        public Guid Id { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
