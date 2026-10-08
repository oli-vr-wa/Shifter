using Shifter.Core.Entities.Identity;

namespace Shifter.Application.DTOs.HumanResources;

public record OnboardEmployeeRequest(
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string? JobPosition,
    DateTime EmploymentStartDate,
    Role Role
);
