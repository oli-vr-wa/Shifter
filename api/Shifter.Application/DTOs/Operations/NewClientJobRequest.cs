using Shifter.Core.Entities.Customers;
using Shifter.Core.Entities.Location;

namespace Shifter.Application.DTOs.Operations;

public record NewClientJobRequest(
    ClientType ClientType,
    string? CompanyName,
    string? Abn,
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhoneNumber,
    Address? Address,
    List<ClientContact>? Contacts
);