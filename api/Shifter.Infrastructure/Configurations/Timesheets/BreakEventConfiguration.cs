using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shifter.Core.Entities.Timesheets;

namespace Shifter.Infrastructure.Configurations.Timesheets;

public class BreakEventConfiguration : IEntityTypeConfiguration<BreakEvent>
{
    public void Configure(EntityTypeBuilder<BreakEvent> builder)
    {
        builder.Property(be => be.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(be => be.Notes)
            .HasMaxLength(600);
    }
}
