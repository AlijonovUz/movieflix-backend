using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Infrastructure.Data.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("Countries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.HasIndex(x => x.Name)
            .IsUnique();
        
        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.HasIndex(x => x.Slug)
            .IsUnique();
    }
}