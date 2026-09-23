using System.ComponentModel.DataAnnotations;
using Domain.Flights;

namespace Api.Dtos;

public record FlightDto(
    Guid Id, 
    string FlightNumber,
    string Origin,
    string Destination,
    DateTime DepartureTime,
    DateTime ArrivalTime,
    int SeatsCount,
    decimal Price,
    string Status)
{
    public static FlightDto FromDomainModel(Flight flight)
        => new(
            flight.Id,
            flight.FlightNumber,
            flight.Origin,
            flight.Destination,
            flight.DepartureTime,
            flight.ArrivalTime,
            flight.SeatsCount,
            flight.Price,
            flight.Status.ToString());
}

public record CreateFlightDto(
    [Required, MaxLength(6)] string FlightNumber,
    [Required, MaxLength(4)] string Origin,
    [Required, MaxLength(4)] string Destination,
    DateTime DepartureTime,
    DateTime ArrivalTime,
    [Range(1, 1_000_000)] int SeatsCount,
    [Range(0.01, 1_000_000)] decimal Price);
    
public record UpdateFlightDto(
    [Required, MaxLength(6)] string FlightNumber,
    [Required, MaxLength(4)] string Origin,
    [Required, MaxLength(4)] string Destination,
    DateTime DepartureTime,
    DateTime ArrivalTime,
    [Range(1, 1_000_000)] int SeatsCount,
    [Range(0.01, 1_000_000)] decimal Price,
    [Required] FlightStatus FlightStatus);