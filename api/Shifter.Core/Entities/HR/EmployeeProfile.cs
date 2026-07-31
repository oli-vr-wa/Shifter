using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.Identity;
using Shifter.Core.Entities.Tenant;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shifter.Core.Entities.HR
{
    public class EmployeeProfile : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
                
        public Guid UserId { get; set; }
        public User? User { get; set; }

        // Multi-tenant lock
        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
