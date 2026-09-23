using Application.Common.Interfaces;
using Domain.Flights;

namespace Infrastructure.Persistance.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly List<Flight> _flights = [];
    private readonly SemaphoreSlim _lock = new(1, 1);
    
    public async Task<IReadOnlyList<Flight>> GetAll(CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            return await Task.FromResult<IReadOnlyList<Flight>>(_flights.ToList());
        }
        finally
        {
            _lock.Release();
        }
        
    }

    public async Task<Flight?> GetById(Guid id, CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            return await Task.FromResult(_flights.FirstOrDefault(f => f.Id == id));
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<Flight?> GetByNumber(string flightNumber, CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            return await Task.FromResult(_flights.FirstOrDefault(f => f.FlightNumber == flightNumber));
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<Flight> Add(Flight flight, CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            _flights.Add(flight);
            return await Task.FromResult(flight);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<Flight> Update(Flight flight, CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            var index = _flights.IndexOf(flight);
            _flights[index] = flight;
            return await Task.FromResult(flight);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task Delete(Flight flight, CancellationToken cancellationToken)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            _flights.Remove(flight);
        }
        finally
        {
            _lock.Release();
        }
    }
}