using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model of user table in Sitefinity database. Table "sf_users".
/// </summary>
[Table("sf_users")]
public partial class User : ISitefinityModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_name")]
    public string? UserName { get; set; }

    [Column("first_name")]
    public string? FirstName { get; set; }

    [Column("last_name")]
    public string? LastName { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("is_backend_user")]
    public bool IsBackendUser { get; set; }
}
