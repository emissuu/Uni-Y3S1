using Application.Common.Interfaces;
using Application.Flights.Services.Abstract;
using Domain.Flights;

namespace Application.Flights.Services.Implementation;

public class FlightService(IFlightRepository flightRepository) : IFlightService
{
    public async Task<IReadOnlyList<Flight>> GetFlights(CancellationToken cancellationToken)
    {
        return await flightRepository.GetAll(cancellationToken);
    }

    public async Task<Flight?> GetFlight(Guid id, CancellationToken cancellationToken)
    {
        return await flightRepository.GetById(id, cancellationToken);
    }

    public async Task<Flight> Add(string flightNumber, string origin, string destination, DateTime departureTime, DateTime arrivalTime, int seatsCount, decimal price, CancellationToken cancellationToken)
    {
        var existingFlight = await flightRepository.GetByNumber(flightNumber, cancellationToken);
        if (existingFlight is not null)
        {
            throw new ArgumentException($"Flight with number {flightNumber} already exists");
        }

        if (origin == destination)
            throw new ArgumentException($"Flight origin and destination cannot be the same");
        
        if (departureTime >= arrivalTime)
            throw new ArgumentException($"Departure time must be before arrival time");
        
        var flight = Flight.New(Guid.NewGuid(), flightNumber, origin, destination, departureTime, arrivalTime, seatsCount, price, FlightStatus.Scheduled);
        return await flightRepository.Add(flight, cancellationToken);
    }

    public async Task<Flight?> Update(Guid id, string flightNumber, string origin, string destination, DateTime departureTime, DateTime arrivalTime, int seatsCount, decimal price, FlightStatus flightStatus, CancellationToken cancellationToken)
    {
        var flight = await flightRepository.GetById(id, cancellationToken);
        if (flight is null)
        {
            return null;
        }
        
        var flightWithTheSameNumber = await flightRepository.GetByNumber(flightNumber, cancellationToken);
        if (flightWithTheSameNumber is not null && flightWithTheSameNumber.Id != flight.Id)
        {
            throw new ArgumentException($"Flight with number {flightNumber} already exists");
        }
        
        if (origin == destination)
            throw new ArgumentException($"Flight origin and destination cannot be the same");
        
        if (departureTime >= arrivalTime)
            throw new ArgumentException($"Departure time must be before arrival time");
            
        flight.UpdateDetails(flightNumber, origin, destination, departureTime, arrivalTime, seatsCount, price, flightStatus);
        return await flightRepository.Update(flight, cancellationToken);
    }

    public async Task<bool> Delete(Guid id, CancellationToken cancellationToken)
    {
        var flight = await flightRepository.GetById(id, cancellationToken);
        if (flight is null)
        {
            return false;
        }
        await flightRepository.Delete(flight, cancellationToken);
        return true;
    }
}