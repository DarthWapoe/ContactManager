using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents a contact with personal and contact information.
/// </summary>
[Table("contacts")]
[Index(nameof(Email), IsUnique = true)]
public class Contact
{
    /// <summary>
    /// Gets or sets the unique identifier for the contact.
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the salutation for the contact.
    /// </summary>
    public required string Salutation { get; set; }

    /// <summary>
    /// Gets or sets the first name of the contact.
    /// </summary>
    public required string Firstname { get; set; }

    /// <summary>
    /// Gets or sets the last name of the contact.
    /// </summary>
    public required string Lastname { get; set; }

    /// <summary>
    /// Gets or sets the display name of the contact.
    /// </summary>
    public string? Displayname { get; set; }

    /// <summary>
    /// Gets or sets the birthdate of the contact.
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime? Birthdate { get; set; } 

    /// <summary>
    /// Gets or sets the creation timestamp of the contact.
    /// </summary>
    public DateTime CreationTimestamp { get; set; } = DateTime.Now; 

    /// <summary>
    /// Gets or sets the last change timestamp of the contact.
    /// </summary>
    public DateTime LastChangeTimestamp { get; set; } = DateTime.Now;

    /// <summary>
    /// Gets a value indicating whether the contact has a birthday soon.
    /// </summary>
    [NotMapped]
    public bool NotifyHasBirthdaySoon
    {
        get
        {
            if (!Birthdate.HasValue)
            {
                return false;
            }

            var nextBirthday = Birthdate.Value.AddYears(DateTime.Now.Year - Birthdate.Value.Year);

            if (nextBirthday < DateTime.Now)
            {
                nextBirthday = nextBirthday.AddYears(1);
            }

            return (nextBirthday - DateTime.Now).TotalDays <= 14;
        }
    }

    /// <summary>
    /// Gets or sets the email address of the contact.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the contact.
    /// </summary>
    public string? Phonenumber { get; set; }
}