namespace Migration.Toolkit.Data.Models
{
    public class SitefinityTypeDefinition
    {
        public required string SitefinityTypeNameSpace { get; set; }
        public required string SitefinityTypeName { get; set; }
        public required Guid DataClassGuid { get; set; }
    }
}
