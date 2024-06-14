namespace Migration.Toolkit.Data.Models
{
    public abstract class SitefinityType : ISitefinityModel
    {
        public string? DisplayName { get; set; }
        public abstract string? Name { get; }
        public Guid Id { get; set; }
        public List<Field>? Fields { get; set; }
        public DateTime? LastModified { get; set; }
        public abstract string? ClassNamespace { get; }
    }
}
