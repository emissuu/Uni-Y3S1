using Domain.Flashcards;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class FlashcardConfiguration : IEntityTypeConfiguration<Flashcard>
{
    public void Configure(EntityTypeBuilder<Flashcard> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Question)
            .HasColumnType("varchar(255)")
            .IsRequired();
        builder.HasIndex(x => x.Question).IsUnique();

        builder.Property(x => x.Hint)
            .HasColumnType("varchar(2047)");
        
        builder.Property(x => x.Answer)
            .HasColumnType("varchar(255)")
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnType("varchar(2047)");

        builder.Property(x => x.Score)
            .HasColumnType("int")
            .IsRequired();
        
        builder.Property(x => x.DueDate)
            .HasConversion(new DateTimeUtcConverter())
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