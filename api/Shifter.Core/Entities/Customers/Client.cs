using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Location;
using Shifter.Core.Entities.Operations;

namespace Shifter.Core.Entities.Customers;

public class Client : BaseEntity, IMultiTenant
{
    public Guid CompanyId { get; set; }
    public required ClientType Type { get; set; }

    // Business Information
    public string? CompanyName { get; set; }
    public string? Abn { get; set; }

    // Individual Information
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    // Contact Information
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    public Guid? AddressId { get; set; }
    public Address? Address { get; set; }

    public string DisplayName => Type == ClientType.Individual ? $"{FirstName} {LastName}" : CompanyName ?? string.Empty;

    public ICollection<ClientContact> Contacts { get; set; } = [];
    public ICollection<Job> Jobs { get; set; } = [];

    /// <summary>
    /// Creates a new instance of the Client class with the specified parameters.
    /// </summary>
    /// <param name="type">The type of the client (Individual or Business).</param>
    /// <param name="companyName">The name of the company (for business clients).</param>
    /// <param name="abn">The Australian Business Number (for business clients).</param>
    /// <param name="firstName">The first name of the individual (for individual clients).</param>
    /// <param name="lastName">The last name of the individual (for individual clients).</param>
    /// <param name="email">The email address of the client.</param>
    /// <param name="phoneNumber">The phone number of the client.</param>
    /// <returns>A new instance of the Client class.</returns>
    public static Client Create(ClientType type, string? companyName, string? abn, string? firstName, string? lastName, string? email, string? phoneNumber)
    {
        var client = new Client
        {
            Type = type,
            CompanyName = companyName,
            Abn = abn,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber
        };

        client.Validate();

        return client;
    }

    /// <summary>
    /// Updates the client information with the specified parameters.
    /// </summary>
    /// <param name="type">The type of the client (Individual or Business).</param>
    /// <param name="companyName">The name of the company (for business clients).</param>
    /// <param name="abn">The Australian Business Number (for business clients).</param>
    /// <param name="firstName">The first name of the individual (for individual clients).</param>
    /// <param name="lastName">The last name of the individual (for individual clients).</param>
    /// <param name="email">The email address of the client.</param>
    /// <param name="phoneNumber">The phone number of the client.</param>
    public void Update(ClientType type, string? companyName, string? abn, string? firstName, string? lastName, string? email, string? phoneNumber)
    {
        Type = type;
        CompanyName = companyName;
        Abn = abn;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;

        Validate();
    }

    /// <summary>
    /// Sets the address for the client.
    /// </summary>
    /// <param name="address">The address to set for the client.</param>
    /// <exception cref="ArgumentNullException">Thrown when the address is null.</exception>
    public void SetAddress(Address address)
    {
        Address = address ?? throw new ArgumentNullException(nameof(address));
    }

    /// <summary>
    /// Adds a contact to the client's list of contacts.
    /// </summary>
    /// <param name="contact">The contact to add to the client's list of contacts.</param>
    /// <exception cref="ArgumentNullException">Thrown when the contact is null.</exception>
    public void AddContact(ClientContact contact)
    {
        ArgumentNullException.ThrowIfNull(contact, nameof(contact));
        Contacts.Add(contact);
    }

    /// <summary>
    /// Validates the client information based on the client type and required fields.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when the client information is invalid.</exception>
    private void Validate()
    {
        if (Type == ClientType.Individual)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(FirstName, nameof(FirstName));
            ArgumentException.ThrowIfNullOrWhiteSpace(LastName, nameof(LastName));
        }
        else
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(CompanyName, nameof(CompanyName));
            ArgumentException.ThrowIfNullOrWhiteSpace(Abn, nameof(Abn));
        }
        if (string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(PhoneNumber))
        {
            throw new ArgumentException("Either email or phone number must be provided.");
        }
    }
}
