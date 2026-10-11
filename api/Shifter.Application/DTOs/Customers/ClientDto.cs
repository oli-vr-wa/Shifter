using Shifter.Application.DTOs.Location;
using Shifter.Core.Entities.Customers;

namespace Shifter.Application.DTOs.Customers;

public record ClientDto(
    Guid Id,
    ClientType Type,
    string? CompanyName,
    string? Abn,
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhoneNumber,
    AddressDto? Address,
    List<ClientContactDto>? Contacts
);
