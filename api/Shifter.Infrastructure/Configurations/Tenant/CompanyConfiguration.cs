using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shifter.Core.Entities.Tenant;

namespace Shifter.Infrastructure.Configurations.Tenant;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.Property(c => c.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Abn)
            .HasMaxLength(11)
            .IsFixedLength()
            .IsUnicode(false)
            .IsRequired();

        builder.HasIndex(c => c.Abn);
    }
}
