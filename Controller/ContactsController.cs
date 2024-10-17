using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for managing contacts.
/// </summary>
[ApiController]
[Produces("application/json")]
[Route("api/v1/[controller]")]
public class ContactsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<ContactsController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactsController"/> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="context">The database context instance.</param>
    public ContactsController(ILogger<ContactsController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    // <snippet_Create>
    /// <summary>
    /// Creating a Contact.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Contact
    ///     {
    ///        "Salutation": "Mr.",
    ///        "Firstname": "John",
    ///        "Lastname": "Doe",
    ///        "Displayname": "Mr. John Doe",
    ///        "Birthdate": "01.01.1900",
    ///        "Email": "john.doe@mail.com",
    ///        "Phonenumber": "0123456789"
    ///     }
    ///
    /// </remarks>
    /// <param name="createContactDto">The created contact details</param>
    /// <returns>A newly created Contact</returns>
    /// <response code="201">Returns the newly created contact</response>
    /// <response code="400">If the contact is null</response>            
    [HttpPost]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateContactDto createContactDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Map CreateContactDto to Contact entity
        var contact = new Contact
        {
            Salutation = createContactDto.Salutation,
            Firstname = createContactDto.Firstname,
            Lastname = createContactDto.Lastname,
            Displayname = createContactDto.Displayname,
            Birthdate = createContactDto.Birthdate,
            Email = createContactDto.Email,
            Phonenumber = createContactDto.Phonenumber
        };
        _context.Contacts.Add(contact);

        try
        {
            _context.SaveChanges();
        }
        catch (Exception)
        {
            //own error message, because of possible database informations
            _logger.LogError($"Error creating contact");
            return BadRequest();
        }

        return CreatedAtRoute("GetContact", new { id = contact.Id }, contact);
    }
    // </snippet_Create>

    // <snippet_Update>
    /// <summary>
    /// Updating an existing Contact.
    /// </summary>
    /// <param name="id">The ID of the contact to update</param>
    /// <param name="updateContactDto">The updated contact details</param>
    /// <response code="204">If the update is successful</response>
    /// <response code="400">If the input is invalid</response>
    /// <response code="404">If the contact is not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid id, [FromBody] UpdateContactDto updateContactDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingContact = _context.Contacts.Find(id);
        if (existingContact == null)
        {
            return NotFound();
        }

        existingContact.Salutation = updateContactDto.Salutation;
        existingContact.Firstname = updateContactDto.Firstname;
        existingContact.Lastname = updateContactDto.Lastname;
        existingContact.Displayname = updateContactDto.Displayname;
        existingContact.Birthdate = updateContactDto.Birthdate;
        existingContact.Email = updateContactDto.Email;
        existingContact.Phonenumber = updateContactDto.Phonenumber;
        existingContact.LastChangeTimestamp = DateTime.Now;

        try
        {
            _context.SaveChanges();
        }
        catch (Exception)
        {
            //own error message, because of database informations
            _logger.LogError($"Error creating contact");
            return BadRequest();
        }

        return CreatedAtRoute("GetContact", new { id = existingContact.Id }, existingContact);
    }
    // </snippet_Update>

    // <snippet_Delete>
    /// <summary>
    /// Deleting an existing Contact.
    /// </summary>
    /// <param name="id">The ID of the contact to delete</param>
    /// <response code="204">If the deletion is successful</response>
    /// <response code="404">If the contact is not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid id)
    {
        var contact = _context.Contacts.Find(id);
        if (contact == null)
        {
            return NotFound();
        }

        _context.Contacts.Remove(contact);

        try
        {
            _context.SaveChanges();
        }
        catch (Exception)
        {
            //own error message, because of database informations
            _logger.LogError($"Error creating contact");
            return BadRequest();
        }

        return NoContent();
    }
    // </snippet_Delete>

    // <snippet_GetById>
    /// <summary>
    /// Reading one Contact by a given Id.
    /// </summary>
    /// <response code="200">Returns contact by given Id</response>
    /// <response code="400">If the input is invalid</response>
    /// <response code="404">If the contact is not found</response>
    [HttpGet("{id}", Name = "GetContact")]
    [Produces("application/json", Type = typeof(Contact))]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid id)
    {
        var contact = _context.Contacts.Find(id);

        if (contact == null)
        {
            return NotFound();
        }

        return Ok(contact);
    }
    // </snippet_GetById>  

    // <snippet_GetAll>
    /// <summary>
    /// Reading all Contacts with optional sorting.
    /// </summary>
    /// <param name="sortBy">The field to sort by (e.g., "Firstname", "Lastname", "Birthdate")</param>
    /// <param name="sortDirection">The sort direction ("asc" for ascending, "desc" for descending)</param>
    /// <response code="200">Returns all contacts</response>
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status200OK)]
    public ActionResult<List<Contact>> GetAll(string sortBy = "lastname", string sortDirection = "asc")
    {
        var contacts = _context.Contacts.AsQueryable();

        // Apply sorting
        contacts = sortBy.ToLower() switch
        {
            "lastname" => sortDirection.ToLower() == "desc" ? contacts.OrderByDescending(c => c.Lastname) : contacts.OrderBy(c => c.Lastname),
            "email" => sortDirection.ToLower() == "desc" ? contacts.OrderByDescending(c => c.Email) : contacts.OrderBy(c => c.Email),
            "birthdate" => sortDirection.ToLower() == "desc" ? contacts.OrderByDescending(c => c.Birthdate) : contacts.OrderBy(c => c.Birthdate),
            _ => sortDirection.ToLower() == "desc" ? contacts.OrderByDescending(c => c.Firstname) : contacts.OrderBy(c => c.Firstname),
        };

        return Ok(contacts.ToList());
    }
    // </snippet_GetAll>    
}