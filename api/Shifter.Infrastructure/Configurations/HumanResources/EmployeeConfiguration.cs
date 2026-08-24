using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shifter.Core.Entities.HumanResources;

namespace Shifter.Infrastructure.Configurations.HumanResources;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(e => e.JobPosition)
            .HasMaxLength(100);

        builder.Property(e => e.EmploymentStartDate)
            .HasColumnType("date");

        builder.Property(e => e.EmploymentTerminationDate)
            .HasColumnType("date");
    }
}
