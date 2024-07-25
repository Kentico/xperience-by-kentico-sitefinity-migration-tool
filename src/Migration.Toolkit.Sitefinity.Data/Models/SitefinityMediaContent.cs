using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model of media content table in Sitefinity database. Table "sf_media_content".
/// </summary>
[Table("sf_media_content")]
public partial class SitefinityMediaContent
{
    /// <summary>
    /// The unique identifier of the media content.
    /// </summary>
    [Key]
    [Column("content_id")]
    public Guid Id { get; set; }

    /// <summary>
    /// The unique identifier of the user who last modified the media content.
    /// </summary>
    [Column("last_modified_by")]
    public Guid LastModifiedBy { get; set; }

    /// <summary>
    /// The unique identifier of the owner of the media content.
    /// </summary>
    [Column("ownr")]
    public Guid Owner { get; set; }
}
