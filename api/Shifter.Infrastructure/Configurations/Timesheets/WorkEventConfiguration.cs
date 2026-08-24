using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shifter.Core.Entities.Timesheets;

namespace Shifter.Infrastructure.Configurations.Timesheets;

public class WorkEventConfiguration : IEntityTypeConfiguration<WorkEvent>
{
    public void Configure(EntityTypeBuilder<WorkEvent> builder)
    {
        builder.Property(we => we.Title)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(we => we.Description)
            .HasMaxLength(600);

        builder.Property(we => we.Notes)
            .HasMaxLength(600);
    }
}
