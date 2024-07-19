using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Migration.Toolkit.Data.Models;
/// <summary>
/// Model of user table in Sitefinity database. Table "sf_users".
/// </summary>
[Table("sf_users")]
public partial class User : ISitefinityModel
{
    /// <summary>
    /// ID of the user.
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Username of the user.
    /// </summary>
    [Column("user_name")]
    public string? UserName { get; set; }

    /// <summary>
    /// First name of the user.
    /// </summary>
    [Column("first_name")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Last name of the user.
    /// </summary>
    [Column("last_name")]
    public string? LastName { get; set; }

    /// <summary>
    /// Email address of the user.
    /// </summary>
    [Column("email")]
    public string? Email { get; set; }

    /// <summary>
    /// A value indicating whether the user is a backend user.
    /// </summary>
    [Column("is_backend_user")]
    public bool IsBackendUser { get; set; }
}
