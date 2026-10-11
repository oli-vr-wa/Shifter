using Shifter.Application.DTOs.Common;
using Shifter.Core.Entities.Customers;

namespace Shifter.Application.DTOs.Customers;

public record ClientQueryFilter : QueryFilterBase
{    
    public ClientType? ClientType { get; set; }
    public bool? HasActiveJobs { get; set; }
}
