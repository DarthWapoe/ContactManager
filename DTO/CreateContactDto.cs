using System.ComponentModel.DataAnnotations;

/// <summary>
/// DTO for creating a contact.
/// </summary>
public class CreateContactDto
{
    /// <summary>
    /// Gets or sets the salutation.
    /// </summary>
    [Required(ErrorMessage = "Salutation is required.")]
    [MinLength(3, ErrorMessage = "Salutation must be longer than 2 characters.")]
    public required string Salutation { get; set; }

    /// <summary>
    /// Gets or sets the first name of the contact.
    /// </summary>
    [Required(ErrorMessage = "Firstname is required.")]
    [MinLength(3, ErrorMessage = "Firstname must be longer than 2 characters.")]
    public required string Firstname { get; set; }

    /// <summary>
    /// Gets or sets the last name of the contact.
    /// </summary>
    [Required(ErrorMessage = "Lastname is required.")]
    [MinLength(3, ErrorMessage = "Lastname must be longer than 2 characters.")]
    public required string Lastname { get; set; }


    private string? _displayname;
    /// <summary>
    /// Gets or sets the display name of the contact.
    /// </summary>
    public string? Displayname { 
        get { 
            return !string.IsNullOrWhiteSpace(_displayname) 
                ? _displayname 
                : $"{Salutation} {Firstname} {Lastname}";
        } 
        set { _displayname = value; } 
    }

    /// <summary>
    /// Gets or sets the birthdate of the contact.
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime? Birthdate { get; set; }

    /// <summary>
    /// Gets or sets the email address of the contact.
    /// </summary>
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email must be in email-formnat.")]
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the phone number of the contact.
    /// </summary>
    [Phone(ErrorMessage = "Invalid phone number.")]
    public string? Phonenumber { get; set; }
}