using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migration.Tookit.Data.Models;
[Table("sf_media_content")]
public partial class SitefinityMediaContent
{
    [Key]
    [Column("content_id")]
    public Guid Id { get; set; }

    [Column("last_modified_by")]
    public Guid LastModifiedBy { get; set; }

    [Column("ownr")]
    public Guid Owner { get; set; }
}
