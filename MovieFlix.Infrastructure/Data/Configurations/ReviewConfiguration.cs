using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Infrastructure.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews", t =>
            t.HasCheckConstraint("CK_Review_Rating",  "Rating IS NULL OR Rating >= 1 AND Rating <= 10"));
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Comment)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne(x => x.Movie)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.Parent)
            .WithMany(r => r.Replies)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}