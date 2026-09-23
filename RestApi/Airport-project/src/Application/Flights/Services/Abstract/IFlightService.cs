using Domain.Flights;

namespace Application.Flights.Services.Abstract;

public interface IFlightService
{
    Task<IReadOnlyList<Flight>> GetFlights(CancellationToken cancellationToken);
    Task<Flight?> GetFlight(Guid id, CancellationToken cancellationToken);
    Task<Flight> Add(
        string flightNumber,
        string origin,
        string destination,
        DateTime departureTime,
        DateTime arrivalTime,
        int seatsCount,
        decimal price,
        CancellationToken cancellationToken);
    Task<Flight?> Update(
        Guid id, 
        string flightNumber,
        string origin,
        string destination,
        DateTime departureTime,
        DateTime arrivalTime,
        int seatsCount,
        decimal price,
        FlightStatus flightStatus,
        CancellationToken cancellationToken);
    Task<bool> Delete(Guid id, CancellationToken cancellationToken);
}