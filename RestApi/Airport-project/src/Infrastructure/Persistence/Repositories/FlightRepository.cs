using Application.Common.Interfaces;
using Domain.Flights;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class FlightRepository(ApplicationDbContext context) : IFlightRepository
{
    public async Task<IReadOnlyList<Flight>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Flights.ToListAsync(cancellationToken);
    }

    public async Task<Flight?> GetById(Guid id, CancellationToken cancellationToken)
    {
        return await context.Flights.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Flight?> GetByNumber(string flightNumber, CancellationToken cancellationToken)
    {
        return await context.Flights.FirstOrDefaultAsync(x => x.FlightNumber == flightNumber, cancellationToken);
    }

    public async Task<Flight> Add(Flight flight, CancellationToken cancellationToken)
    {
        await context.Flights.AddAsync(flight, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return flight;
    }

    public async Task<Flight> Update(Flight flight, CancellationToken cancellationToken)
    {
        var existingFlight = await context.Flights.FirstOrDefaultAsync(x => x.Id == flight.Id, cancellationToken);

        if (existingFlight == null)
        {
            return null;
        }
        
        context.Entry(existingFlight).CurrentValues.SetValues(flight);
        
        await context.SaveChangesAsync(cancellationToken);
        return existingFlight;
    }

    public async Task Delete(Flight flight, CancellationToken cancellationToken)
    {
        var existingFlight = await context.Flights.FirstOrDefaultAsync(x => x.Id == flight.Id, cancellationToken);
        if (existingFlight == null)
        {
            return;
        }
        context.Flights.Remove(existingFlight);
        await context.SaveChangesAsync(cancellationToken);
    }
}