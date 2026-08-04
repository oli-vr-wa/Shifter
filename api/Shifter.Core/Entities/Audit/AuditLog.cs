using Shifter.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Core.Entities.Audit;

public class AuditLog : BaseEntity, IMultiTenant
{
    public Guid CompanyId { get; set; }

    public required string EntityName { get; set; }
    public required string EntityId { get; set; }
    public required string Action { get; set; }
    public Guid? UserProfileId { get; set; } // The ID of the user who performed the action

    public string Changes { get; set; } = string.Empty; // A JSON string representing the changes made to the entity using jsonb
}
