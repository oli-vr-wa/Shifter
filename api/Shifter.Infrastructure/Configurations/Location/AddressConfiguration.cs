using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shifter.Core.Entities.Location;

namespace Shifter.Infrastructure.Configurations.Location;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.Property(a => a.PlaceId)
            .HasMaxLength(500)
            .IsRequired();        

        builder.Property(a => a.StreetNumber)
            .HasMaxLength(50);

        builder.Property(a => a.StreetName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.Suburb)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(a => a.State)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(a => a.PostCode)
            .HasMaxLength(12)
            .IsUnicode(false)
            .IsRequired();

        builder.Property(a => a.Country)
            .HasMaxLength(60)
            .IsRequired();

        builder.HasIndex(a => a.PlaceId);
        builder.HasIndex(a => a.Suburb);
        builder.HasIndex(a => a.PostCode);
    }
}
