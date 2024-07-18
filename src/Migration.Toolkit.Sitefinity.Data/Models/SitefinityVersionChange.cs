using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model of version changes table in Sitefinity database. Table "sf_version_chnges".
/// </summary>
[Table("sf_version_chnges")]
public partial class SitefinityVersionChange
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Column("item_id")]
    public Guid ItemId { get; set; }
    [Column("ownr")]
    public Guid Owner { get; set; }
    [Column("last_modified")]
    public DateTime LastModified { get; set; }
    [Column("date_created")]
    public DateTime DateCreated { get; set; }
    [Column("vrsion")]
    public int Version { get; set; }
    [Column("change_type")]
    public required string ChangeType { get; set; }
}
