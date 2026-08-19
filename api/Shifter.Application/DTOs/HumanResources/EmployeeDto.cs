namespace Shifter.Application.DTOs.HumanResources;

public record EmployeeDto(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string? JobPosition,
    DateTime EmploymentStartDate
);
