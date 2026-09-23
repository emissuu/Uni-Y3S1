using Domain.Flights;

namespace Application.Common.Interfaces;

public interface IFlightRepository
{
    Task<IReadOnlyList<Flight>> GetAll(CancellationToken cancellationToken);
    Task<Flight?> GetById(Guid id, CancellationToken cancellationToken);
    Task<Flight?> GetByNumber(string flightNumber, CancellationToken cancellationToken);
    Task<Flight> Add(Flight flight, CancellationToken cancellationToken);
    Task<Flight> Update(Flight flight, CancellationToken cancellationToken);
    Task Delete(Flight flight, CancellationToken cancellationToken);
}