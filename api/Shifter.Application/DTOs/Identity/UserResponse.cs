using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Application.DTOs.Identity;

public record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email
);
