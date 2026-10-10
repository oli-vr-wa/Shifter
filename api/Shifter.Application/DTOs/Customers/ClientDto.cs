using Shifter.Application.DTOs.Location;

namespace Shifter.Application.DTOs.Customers;

public record ClientDto(
    Guid Id,
    string? CompanyName,
    string? Abn,
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhoneNumber,
    AddressDto? Address,
    List<ClientContactDto>? Contacts
);
