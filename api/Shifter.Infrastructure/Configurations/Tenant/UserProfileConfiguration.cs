using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shifter.Core.Entities.Tenant;

namespace Shifter.Infrastructure.Configurations.Tenant;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.Property(up => up.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(up => up.MiddleName)
            .HasMaxLength(100);

        builder.Property(up => up.LastName)
            .HasMaxLength(100)
            .IsRequired();
    }
}
