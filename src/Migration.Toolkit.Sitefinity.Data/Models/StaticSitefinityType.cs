namespace Migration.Toolkit.Data.Models
{
    public class StaticSitefinityType : SitefinityType
    {
        public string? ClassName { get; set; }
        public override string? Name => ClassName;
    }
}
