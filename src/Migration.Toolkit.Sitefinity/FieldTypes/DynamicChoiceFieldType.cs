using System.Xml.Linq;

using CMS.Helpers;

using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core;

namespace Migration.Toolkit.Sitefinity.FieldTypes;
public class DynamicChoiceFieldType : IFieldType
{
    public string SitefinityWidgetTypeName => "Telerik.Sitefinity.Web.UI.Fields.DynamicChoiceField";

    public string? GetColumnSize(Field sitefinityField) => sitefinityField.DBLength;
    public string GetColumnType(Field sitefinityField) => "text";

    public FormFieldSettings GetSettings(Field sitefinityField)
    {
        var options = new List<string>();

        if (sitefinityField.Choices == null)
        {
            return Default(options);
        }

        var xmlDoc = XDocument.Parse(sitefinityField.Choices);
        xmlDoc.Element("choices")?.Descendants("choice").ToList().ForEach(item =>
        {
            string option = "";
            if (item.Attribute("value") != null)
            {
                option += item.Attribute("value")?.Value;
            }

            if (item.Attribute("value") != null && item.Attribute("text") != null)
            {
                option += ";";
            }

            if (item.Attribute("text") != null)
            {
                option += item.Attribute("text")?.Value;
            }

            options.Add(option);
        });

        if (sitefinityField.ChoiceRenderType == null)
        {
            return Default(options);
        }

        if (sitefinityField.ChoiceRenderType.Equals("DropDownList"))
        {
            return new FormFieldSettings
            {
                ControlName = "Kentico.Administration.DropDownSelector",
                CustomProperties = new()
                {
                    { "OptionsValueSeparator", ";" },
                    { "Options", options.Join("\r\n") }
                }
            };
        }

        if (sitefinityField.ChoiceRenderType.Equals("RadioButton"))
        {
            return new FormFieldSettings
            {
                ControlName = "Kentico.Administration.RadioGroup",
                CustomProperties = new()
                {
                    { "Inline", "False" },
                    { "OptionsValueSeparator", ";" },
                    { "Options", options.Join("\r\n") }
                }
            };
        }

        return Default(options);
    }

    private FormFieldSettings Default(IEnumerable<string> options) => new()
    {
        ControlName = "Kentico.Administration.DropDownSelector",
        CustomProperties = new()
        {
            { "OptionsValueSeparator", ";" },
            { "Options", options.Join("\r\n") }
        }
    };
}
