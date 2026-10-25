using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Infrastructure.Data.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres");
        
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