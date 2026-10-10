namespace Shifter.Application.DTOs.Customers;

public record ClientContactDto(
    Guid Id,
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhoneNumber
);
