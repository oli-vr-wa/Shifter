using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shifter.Core.Entities.Timesheets;

namespace Shifter.Infrastructure.Configurations.Timesheets;

public class WorkEventHistoryConfiguration : IEntityTypeConfiguration<WorkEventHistory>
{
    public void Configure(EntityTypeBuilder<WorkEventHistory> builder)
    {
        builder.Property(weh => weh.Description)
            .HasMaxLength(1000);
    }
}
