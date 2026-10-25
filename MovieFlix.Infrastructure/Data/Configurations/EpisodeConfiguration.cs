using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MovieFlix.Domain.Entities;

namespace MovieFlix.Infrastructure.Data.Configurations;

public class EpisodeConfiguration : IEntityTypeConfiguration<Episode>
{
    public void Configure(EntityTypeBuilder<Episode> builder)
    {
        builder.ToTable("Episodes", t =>
            t.HasCheckConstraint("CK_Episode_Number", "Number IS NULL OR Number > 0"));
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Number);

        builder.Property(x => x.Title)
            .HasMaxLength(256);
        
        builder.Property(x => x.VideoUrl)
            .IsRequired()
            .HasMaxLength(5000);

        builder.HasOne(x => x.Movie)
            .WithMany(m => m.Episodes)
            .HasForeignKey(x => x.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}