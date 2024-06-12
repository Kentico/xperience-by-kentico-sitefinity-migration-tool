namespace Migration.Toolkit.Sitefinity.Helpers;
public static class FieldHelper
{
    public static string MapControlType(string? sitefinityControlType)
    {
#pragma warning disable IDE0066 // Convert switch statement to expression
        switch (sitefinityControlType)
        {
            case "Telerik.Sitefinity.Web.UI.Fields.TextField":
                return "Kentico.Administration.TextInput";
            case "Telerik.Sitefinity.Web.UI.Fields.HtmlField":
                // Handle long text field
                return "";
            case "Telerik.Sitefinity.Web.UI.Fields.CheckboxField":
                // Handle yes/no field
                return "";
            case "Telerik.Sitefinity.Web.UI.Fields.ChoiceField":
                // Handle choice field
                return "Kentico.Administration.Checkbox";
            case "Telerik.Sitefinity.Web.UI.Fields.DateTimeField":
                // Handle date/time field
                return "";
            case "Telerik.Sitefinity.Web.UI.Fields.DecimalField":
                // Handle number or currency field
                return "";
            case "Telerik.Sitefinity.Web.UI.Fields.GuidField":
                // Handle guid field
                return "";
            case "Telerik.Sitefinity.Web.UI.Fields.AddressField":
                // Handle address field
                return "";
            case "Telerik.Sitefinity.Web.UI.Fields.ImageField":
                // Handle image field
                return "";
            case "Telerik.Sitefinity.Web.UI.Fields.FileField":
                // Handle file field
                return "";
            case "Telerik.Sitefinity.Web.UI.Fields.VideoField":
                // Handle video field
                return "";
            case "Telerik.Sitefinity.Web.UI.Fields.RelatedDataField":
                // Handle content items or related data field
                return "";
            case "Telerik.Sitefinity.Web.UI.Fields.TaxonField":
                // Handle classification field
                return "";
            case "Telerik.Sitefinity.Web.UI.Fields.RelatedMediaField":
                // Handle related media field
                return "";
            case "Telerik.Sitefinity.Web.UI.Fields.UrlField":
                // Handle URL field
                return "";
            case "Telerik.Sitefinity.Web.UI.Fields.GeolocationField":
                // Handle geolocation field
                return "";
            default:
                // Handle unknown field type
                return "Kentico.Administration.TextInput";
        }
#pragma warning restore IDE0066 // Convert switch statement to expression
    }

    public static string MapColumnType(string? sitefinityColumnType)
    {
#pragma warning disable IDE0066 // Convert switch statement to expression
        switch (sitefinityColumnType)
        {
            case "NVARCHAR":
                // Handle NVARCHAR column
                return "text";
            case "NTEXT":
                // Handle NTEXT column
                return "text";
            case "BIT":
                // Handle BIT column
                return "boolean";
            case "DATETIME":
                // Handle DATETIME column
                return "datetime";
            case "FLOAT":
                // Handle FLOAT column
                return "double";
            case "INTEGER":
                // Handle INTEGER column
                return "integer";
            case "MONEY":
                // Handle MONEY column
                return "decimal";
            case "UNIQUEIDENTIFIER":
                // Handle UNIQUEIDENTIFIER column
                return "guid";
            default:
                // Handle unknown column type
                return "text";
        }
#pragma warning restore IDE0066 // Convert switch statement to expression
    }
}
