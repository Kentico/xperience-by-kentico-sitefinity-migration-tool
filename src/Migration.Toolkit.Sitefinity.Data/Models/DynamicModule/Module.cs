namespace Migration.Tookit.Data.Models
{
    public class Module
    {
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public List<DynamicModuleType>? Types { get; set; }
        public object? DefaultBackendDefinitionName { get; set; }
        public string? PageId { get; set; }
        public string? UrlName { get; set; }
        public string? Id { get; set; }
        public string? Owner { get; set; }
    }
}
