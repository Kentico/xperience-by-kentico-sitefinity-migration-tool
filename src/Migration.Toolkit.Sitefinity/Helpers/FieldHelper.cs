namespace Migration.Toolkit.Sitefinity.Helpers;
public static class FieldHelper
{
    public static string MapColumnType(string? sitefinityColumnType) => sitefinityColumnType switch
    {
        "NVARCHAR" => "text",
        "NTEXT" => "text",
        "CLOB" => "longtext",
        "BIT" => "boolean",
        "DATE" => "datetime",
        "FLOAT" => "double",
        "INTEGER" => "integer",
        "MONEY" => "decimal",
        "UNIQUEIDENTIFIER" => "guid",
        _ => "text",
    };
}
