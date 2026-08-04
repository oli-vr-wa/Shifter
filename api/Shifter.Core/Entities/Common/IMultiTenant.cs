
namespace Shifter.Core.Entities.Common;

public interface IMultiTenant
{
    Guid CompanyId { get; set; }
}
