﻿using Kentico.Xperience.UMT.Model;

using Migration.Toolkit.Data.Models;
using Migration.Toolkit.Sitefinity.Core;

namespace Migration.Toolkit.Sitefinity.FieldTypes;
public class LinkFieldType : IFieldType
{
    public string SitefinityWidgetTypeName => "Telerik.Sitefinity.Web.UI.Fields.ComplexLinkField";

    public string? GetColumnSize(Field sitefinityField) => sitefinityField.DBLength;
    public string GetColumnType(Field sitefinityField) => "longtext";
    public FormFieldSettings GetSettings(Field sitefinityField) => new()
    {
        ControlName = "Kentico.Administration.TextArea"
    };
}