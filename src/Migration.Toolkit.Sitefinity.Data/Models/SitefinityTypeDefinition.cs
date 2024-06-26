namespace Migration.Tookit.Data.Models
{
    public class SitefinityTypeDefinition
    {
        public required string SitefinityTypeName { get; set; }
        public required Guid DataClassGuid { get; set; }
        public IEnumerable<string>? RelatedFields { get; set; }
    }
}
