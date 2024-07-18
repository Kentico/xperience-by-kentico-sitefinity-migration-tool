using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model of page node table in Sitefinity database. Table "sf_page_node".
/// </summary>
[Table("sf_page_node")]
public partial class SitefinityPageNode
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Column("ownr")]
    public Guid Owner { get; set; }
    [Column("date_created")]
    public DateTime DateCreated { get; set; }
}
