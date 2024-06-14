using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Models;

namespace Migration.Toolkit.Sitefinity.Core;
public interface IFieldType
{
    public string SitefinityWidgetTypeName { get; }
    public string GetColumnType(Field sitefinityField);
    public string? GetColumnSize(Field sitefinityField);
    public FormFieldSettings GetSettings(Field sitefinityField);
}
