using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Infrastructure.Data.Configurations;

public class AgeLimitConfiguration : IEntityTypeConfiguration<AgeLimit>
{
    public void Configure(EntityTypeBuilder<AgeLimit> builder)
    {
        builder.ToTable("AgeLimits", t =>
            t.HasCheckConstraint("CK_AgeLimit_Age", "Age >= 0 AND Age <= 25"));
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Age)
            .IsRequired();
        
        builder.HasIndex(x => x.Age)
            .IsUnique();
    }
}