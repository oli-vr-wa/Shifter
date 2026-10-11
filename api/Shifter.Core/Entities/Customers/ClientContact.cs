using Shifter.Core.Entities.Common;

namespace Shifter.Core.Entities.Customers;

public class ClientContact : BaseEntity, IMultiTenant
{
    public Guid CompanyId { get; set; }

    public Guid ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? JobTitle { get; set; }

    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    public bool IsPrimary { get; set; } = false;

    /// <summary>
    /// Creates a new instance of ClientContact with the provided details.
    /// </summary>
    /// <param name="clientId">The unique identifier of the client.</param>
    /// <param name="firstName">The first name of the contact.</param>
    /// <param name="lastName">The last name of the contact.</param>
    /// <param name="jobTitle">The job title of the contact.</param>
    /// <param name="email">The email address of the contact.</param>
    /// <param name="phoneNumber">The phone number of the contact.</param>
    /// <param name="isPrimary">Indicates whether the contact is the primary contact.</param>
    /// <returns>A new instance of the ClientContact class.</returns>
    /// <exception cref="ArgumentException">Thrown when neither email nor phone number is provided.</exception>
    public static ClientContact Create(Guid clientId, string firstName, string lastName, string? jobTitle, string? email, string? phoneNumber, bool isPrimary = false)
    {        
        var clientContact = new ClientContact
        {
            ClientId = clientId,
            FirstName = firstName,
            LastName = lastName,
            JobTitle = jobTitle,
            Email = email,
            PhoneNumber = phoneNumber,
            IsPrimary = isPrimary
        };

        clientContact.Validate();
        return clientContact;
    }

    /// <summary>
    /// Updates the properties of the ClientContact instance with the provided values.
    /// </summary>
    /// <param name="firstName">The first name of the contact.</param>
    /// <param name="lastName">The last name of the contact.</param>
    /// <param name="jobTitle">The job title of the contact.</param>
    /// <param name="email">The email address of the contact.</param>
    /// <param name="phoneNumber">The phone number of the contact.</param>
    /// <param name="isPrimary">Indicates whether the contact is the primary contact.</param>
    public void Update(string firstName, string lastName, string? jobTitle, string? email, string? phoneNumber, bool isPrimary = false)
    {
        FirstName = firstName;
        LastName = lastName;
        JobTitle = jobTitle;
        Email = email;
        PhoneNumber = phoneNumber;
        IsPrimary = isPrimary;
        Validate();
    }

    /// <summary>
    /// Validates the properties of the ClientContact instance to ensure that required fields are provided.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when required fields are not provided.</exception>
    private void Validate()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(FirstName, nameof(FirstName));
        ArgumentException.ThrowIfNullOrWhiteSpace(LastName, nameof(LastName));
        if (string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(PhoneNumber))
        {
            throw new ArgumentException("Either email or phone number must be provided.");
        }
    }
}
