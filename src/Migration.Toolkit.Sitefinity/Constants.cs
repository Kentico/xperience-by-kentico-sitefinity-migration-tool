namespace Migration.Toolkit.Sitefinity;

internal static class Constants
{
    public static string[] ExcludedFields => ["Translations", "Actions", "DateCreated", "Author", "PublicationDate", "LastModified", "DateCreated"];
    public static string[] ForcedWebsiteTypes => ["PageNode"];
}
