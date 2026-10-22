using Domain.Problems;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class ProblemConfiguration : IEntityTypeConfiguration<Problem>
{
    public void Configure(EntityTypeBuilder<Problem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(x => x.Value, x => new ProblemId(x));
        
        builder.Property(x => x.Title)
            .HasColumnType("varchar(255)")
            .IsRequired();
        builder.HasIndex(x => x.Title).IsUnique();
        
        builder.Property(x => x.Code)
            .HasColumnType("varchar(2047)")
            .IsRequired();
        
        builder.Property(x => x.Answer)
            .HasColumnType("varchar(255)")
            .IsRequired();
        
        builder.Property(x => x.Category)
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .IsRequired();
        
        builder.Property(x => x.Difficulty)
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .IsRequired();
        
        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasConversion(new DateTimeUtcConverter());
        
        builder.Property(x => x.DeletedAt)
            .HasConversion(new DateTimeUtcConverter());
    }
}