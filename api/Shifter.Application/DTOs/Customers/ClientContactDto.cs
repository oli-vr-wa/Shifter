namespace Shifter.Application.DTOs.Customers;

public record ClientContactDto(
    Guid Id,
    string FirstName,
    string LastName,
    string? JobTitle,
    string? Email,
    string? PhoneNumber
);
