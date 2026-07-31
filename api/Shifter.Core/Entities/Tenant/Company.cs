using Shifter.Core.Entities.Common;
using Shifter.Core.Entities.HR;
using Shifter.Core.Entities.Identity;

namespace Shifter.Core.Entities.Tenant
{
    public class Company : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
