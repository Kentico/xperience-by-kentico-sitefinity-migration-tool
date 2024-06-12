namespace Migration.Tookit.Data.Models
{
    public class Section
    {
        public string? Name { get; set; }
        public int Ordinal { get; set; }
        public string? Title { get; set; }
        public bool IsExpandable { get; set; }
        public bool IsExpandedByDefault { get; set; }
        public string? ParentTypeId { get; set; }
        public string? Id { get; set; }
    }
}
