using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model of version changes table in Sitefinity database. Table "sf_version_chnges".
/// </summary>
[Table("sf_version_chnges")]
public partial class SitefinityVersionChange
{
    /// <summary>
    /// The unique identifier of the version change.
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// The identifier of the item associated with the version change.
    /// </summary>
    [Column("item_id")]
    public Guid ItemId { get; set; }

    /// <summary>
    /// The owner of the version change.
    /// </summary>
    [Column("ownr")]
    public Guid Owner { get; set; }

    /// <summary>
    /// The date and time when the version change was last modified.
    /// </summary>
    [Column("last_modified")]
    public DateTime LastModified { get; set; }

    /// <summary>
    /// The date and time when the version change was created.
    /// </summary>
    [Column("date_created")]
    public DateTime? DateCreated { get; set; }

    /// <summary>
    /// The version number of the version change.
    /// </summary>
    [Column("vrsion")]
    public int Version { get; set; }

    /// <summary>
    /// The type of change for the version change.
    /// </summary>
    [Column("change_type")]
    public required string ChangeType { get; set; }
}
