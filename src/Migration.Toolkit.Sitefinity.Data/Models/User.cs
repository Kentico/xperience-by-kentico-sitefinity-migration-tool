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
    /// Email address of the user. If not set, falls back to a default value.
    /// </summary>
    /// <remarks>
    /// kentico requires email address, some sitefinity users do not have an email so they will default to "{first}/unknown.{last}/unknown.{Id}@default-xbyk-migration.local"
    /// </remarks>
    [Column("email")]
    public string? Email
    {
        get
        {
            if (!string.IsNullOrEmpty(email))
            {
                return email;
            }
            string first = string.IsNullOrEmpty(FirstName) ? "unknown" : FirstName;
            string last = string.IsNullOrEmpty(LastName) ? "unknown" : LastName;
            return $"{first}.{last}.{Id}@default-xbyk-migration.local";
        }
        set => email = value;
    }

    private string? email;
    /// <summary>  
    /// A value indicating whether the user is a backend user.  
    /// </summary>  
    [Column("is_backend_user")]
    public bool IsBackendUser { get; set; }
}
