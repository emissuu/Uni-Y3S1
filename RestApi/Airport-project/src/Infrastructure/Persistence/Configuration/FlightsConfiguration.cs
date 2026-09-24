using Domain.Flights;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class FlightsConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FlightNumber)
            .HasColumnType("varchar(6)")
            .IsRequired();
        builder.HasIndex(x => x.FlightNumber).IsUnique();
        
        builder.Property(x => x.Origin)
            .HasColumnType("varchar(4)")
            .IsRequired();
        
        builder.Property(x => x.Destination)
            .HasColumnType("varchar(4)")
            .IsRequired();
        
        builder.Property(x => x.DepartureTime)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired();
        
        builder.Property(x => x.ArrivalTime)
            .HasConversion(new DateTimeUtcConverter())
            .IsRequired();
        
        builder.Property(x => x.SeatsCount)
            .HasColumnType("int")
            .IsRequired();
        
        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired();
        
        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasColumnType("varchar(50)")
            .IsRequired();
    }
}