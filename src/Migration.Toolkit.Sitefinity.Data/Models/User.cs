using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

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
    public string? UserName
    {
        get => userName = GetUserName();
        set => userName = value;
    }

    private string? userName;

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
        get => email = GetEmail();
        set => email = value;
    }

    private string? email;

    /// <summary>  
    /// A value indicating whether the user is a backend user.  
    /// </summary>  
    [Column("is_backend_user")]
    public bool IsBackendUser { get; set; }

    /// <summary>
    /// Gets the email address with fallback logic.
    /// </summary>
    /// <returns>Valid email address or generated fallback email</returns>
    public string GetEmail()
    {
        if (IsValidEmail(email))
        {
            return email!;
        }
        return GenerateFallbackEmail(Id);
    }

    /// <summary>
    /// Gets the username with fallback logic.
    /// </summary>
    /// <returns>Valid username or email address as fallback</returns>
    public string GetUserName()
    {
        // If username is empty, use email>id
        if (string.IsNullOrEmpty(userName))
        {
            return Id.ToString();
        }

        // If username is invalid for Kentico, use email>id
        if (!IsValidKenticoUsername(userName))
        {
            return Id.ToString();
        }

        return userName;
    }

    /// <summary>
    /// Generates a fallback email address for users without a valid email.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>A fallback email address in the format: {id}.default-xbyk-migration@localhost.local</returns>
    public static string GenerateFallbackEmail(Guid id) => $"{id}.default-xbyk-migration@localhost.local";

    /// <summary>
    /// Validates if a username is valid for Kentico by checking if it contains only allowed characters.
    /// Kentico allows: alphanumeric characters, underscore (_), hyphen (-), period (.), and at symbol (@).
    /// </summary>
    /// <param name="username">The username to validate.</param>
    /// <returns>True if the username is valid for Kentico, false otherwise.</returns>
    private static bool IsValidKenticoUsername(string? username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return false;
        }

        // Check if username contains only valid characters for Kentico
        return Regex.IsMatch(username, @"^[a-zA-Z0-9_\-\.@]+$");
    }

    /// <summary>
    /// Validates if the provided email address is a valid email format.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>True if the email is valid, false otherwise.</returns>
    private static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }

        if (!Regex.IsMatch(email, @"^[a-zA-Z0-9_\-\.@]+$"))
        {
            return false;
        }

        var emailAttribute = new EmailAddressAttribute();
        return emailAttribute.IsValid(email);
    }
}
