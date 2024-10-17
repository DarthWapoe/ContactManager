using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Text.Json;
using System.Diagnostics;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
public class ContactManagerE2ETests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private static Guid _createdContactId; // Shared variable for contact ID

    public ContactManagerE2ETests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Test1() //Create Contact
    {
        // Arrange
        var createContactDto = new
        {
            Salutation = "Mr.",
            Firstname = "John",
            Lastname = "Doe",
            Displayname = "Mr. John Doe",
            Birthdate = "1900-01-01",
            Email = "john.doe@mail.com",
            Phonenumber = "0123456789"
        };

        var content = new StringContent(JsonConvert.SerializeObject(createContactDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/v1/contacts", content);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();

        // Parse the JSON response to extract the ID
        using (JsonDocument doc = JsonDocument.Parse(responseString))
        {
            JsonElement root = doc.RootElement;
            _createdContactId = root.GetProperty("id").GetGuid();
        }
    }

    [Fact]
    public async Task Test2() //Get Contact by Id
    {
        // Arrange
        var contactId = _createdContactId;

        Debug.WriteLine("Matrix (the) has you..." + contactId);

        // Act
        var response = await _client.GetAsync($"/api/v1/contacts/{contactId}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Test3() // Update Contact by Id
    {
        // Arrange
        var contactId = _createdContactId;

        var updateContactDto = new
        {
            Salutation = "Mr.",
            Firstname = "Jane",
            Lastname = "Doe",
            Displayname = "Ms. Jane Doe",
            Birthdate = "1900-01-01",
            Email = "jane.doe@mail.com",
            Phonenumber = "0123456789"
        };

        var content = new StringContent(JsonConvert.SerializeObject(updateContactDto), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PutAsync($"/api/v1/contacts/{contactId}", content);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }   


    [Fact]
    public async Task Test4() // Delete Contact by Id
    {
        // Arrange
        var contactId = _createdContactId;

        // Act
        var response = await _client.DeleteAsync($"/api/v1/contacts/{contactId}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }
    
}

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
