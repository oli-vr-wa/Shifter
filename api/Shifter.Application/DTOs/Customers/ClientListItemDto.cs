using Shifter.Core.Entities.Customers;

namespace Shifter.Application.DTOs.Customers;

public record ClientListItemDto(
    Guid Id,
    ClientType Type,
    string DisplayName,
    string? Email,
    string? PhoneNumber,
    string? Address
);
