
namespace Shifter.Core.Entities.Common
{
    public abstract class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedAt { get; set; }
    }
}
