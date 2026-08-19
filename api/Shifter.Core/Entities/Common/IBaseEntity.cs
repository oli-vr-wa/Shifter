namespace Shifter.Core.Entities.Common;

public interface IBaseEntity
{
    DateTime CreatedAt { get; set; }
    DateTime? LastUpdatedAt { get; set; }
}
