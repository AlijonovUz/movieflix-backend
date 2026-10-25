using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Infrastructure.Data.Configurations;

public class ReleaseYearConfiguration : IEntityTypeConfiguration<ReleaseYear>
{
    public void Configure(EntityTypeBuilder<ReleaseYear> builder)
    {
        builder.ToTable("ReleaseYears", t =>
            t.HasCheckConstraint("CK_ReleaseYear_Year", "Year >= 1895"));
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Year)
            .IsRequired();
        
        builder.HasIndex(x => x.Year)
            .IsUnique();
    }
}