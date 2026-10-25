using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Username)
            .IsRequired()
            .HasMaxLength(32);

        builder.HasIndex(x => x.Username)
            .IsUnique();

        builder.Property(x => x.FirstName)
            .HasMaxLength(50);
        
        builder.Property(x => x.LastName)
            .HasMaxLength(50);

        builder.Property(x => x.Email)
            .HasMaxLength(100);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.Role)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(20);
    }
}