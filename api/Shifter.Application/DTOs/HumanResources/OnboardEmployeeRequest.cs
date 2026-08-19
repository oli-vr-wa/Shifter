namespace Shifter.Application.DTOs.HumanResources;

public record OnboardEmployeeRequest(
    string FirstName,
    string LastName,
    string Email,
    string? JobPosition,
    DateTime EmploymentStartDate
);
