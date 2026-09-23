using Api.Dtos;
using Application.Flights.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("flights")]
[ApiController]
public class FlightsController(IFlightService flightService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FlightDto>>> GetFlights(CancellationToken cancellationToken)
    {
        var flights = await flightService.GetFlights(cancellationToken);
        return flights.Select(f => FlightDto.FromDomainModel(f)).ToList();
    }

    [HttpGet("{flightId:guid}")]
    public async Task<ActionResult<FlightDto>> GetFlight(Guid flightId, CancellationToken cancellationToken)
    {
        var flight = await flightService.GetFlight(flightId, cancellationToken);
        if (flight is null)
        {
            return NotFound();
        }

        return FlightDto.FromDomainModel(flight);
    }

    [HttpPost]
    public async Task<ActionResult<FlightDto>> CreateFlight(
        [FromBody] CreateFlightDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var flight = await flightService.Add(
                request.FlightNumber,
                request.Origin,
                request.Destination,
                request.DepartureTime,
                request.ArrivalTime,
                request.SeatsCount,
                request.Price,
                cancellationToken);

            var flightDto = FlightDto.FromDomainModel(flight);
            return CreatedAtAction(nameof(GetFlight), new { flightId = flight.Id }, flightDto);
        }
        catch (ArgumentException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpPut("{flightId:guid}")]
    public async Task<ActionResult<FlightDto>> UpdateFlight(
        Guid flightId,
        [FromBody] UpdateFlightDto request,
        CancellationToken cancellationToken)
    {
        try
        {
            var flight = await flightService.Update(
                flightId,
                request.FlightNumber,
                request.Origin,
                request.Destination,
                request.DepartureTime,
                request.ArrivalTime,
                request.SeatsCount,
                request.Price,
                request.FlightStatus,
                cancellationToken);
            
            if (flight is null)
            {
                return NotFound($"Flight with id {flightId} not found");
            }

            var flightDto = FlightDto.FromDomainModel(flight);
            return Ok(flightDto);
        }
        catch (ArgumentException exception)
        {
            return Conflict(exception.Message);
        }
    }

    [HttpDelete("{flightId:guid}")]
    public async Task<ActionResult> DeleteFlight(Guid flightId, CancellationToken cancellationToken)
    {
        var isDeleted = await flightService.Delete(flightId, cancellationToken);
        if (isDeleted)
        {
            return NoContent();
        }
        return NotFound();
    }
}