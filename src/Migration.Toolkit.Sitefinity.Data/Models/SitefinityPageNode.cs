using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model of page node table in Sitefinity database. Table "sf_page_node".
/// </summary>
[Table("sf_page_node")]
public partial class SitefinityPageNode
{
    /// <summary>
    /// The unique identifier of the page node.
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// The owner of the page node.
    /// </summary>
    [Column("ownr")]
    public Guid Owner { get; set; }

    /// <summary>
    /// The date when the page node was created.
    /// </summary>
    [Column("date_created")]
    public DateTime DateCreated { get; set; }
}
