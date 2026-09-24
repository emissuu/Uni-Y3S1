namespace Domain.Flights;

public class Flight
{
    public Guid Id { get; }
    public string FlightNumber { get; private set; }
    public string Origin { get; private set; }
    public string Destination { get; private set; }
    public DateTime DepartureTime { get; private set; }
    public DateTime ArrivalTime { get; private set; }
    public int SeatsCount { get; private set; }
    public decimal Price { get; private set; }
    public FlightStatus Status { get; private set; }
    
    private Flight(
        Guid id, 
        string flightNumber,
        string origin,
        string destination,
        DateTime departureTime,
        DateTime arrivalTime,
        int seatsCount,
        decimal price,
        FlightStatus status
    ) => 
        (Id, FlightNumber, Origin,  Destination, DepartureTime, ArrivalTime, SeatsCount, Price, Status) = 
        (id, flightNumber, origin, destination, departureTime, arrivalTime, seatsCount, price, status);

    public static Flight New(
        Guid id,
        string flightNumber,
        string origin,
        string destination,
        DateTime departureTime,
        DateTime arrivalTime,
        int seatsCount,
        decimal price,
        FlightStatus status
    ) => new(id, flightNumber, origin, destination, departureTime, arrivalTime, seatsCount, price, status);
    
    public void UpdateDetails(
        string flightNumber,
        string origin,
        string destination,
        DateTime departureTime,
        DateTime arrivalTime,
        int seatsCount,
        decimal price,
        FlightStatus status
    ) => 
        (FlightNumber, Origin, Destination, DepartureTime, ArrivalTime, SeatsCount, Price, Status) =
        (flightNumber, origin, destination, departureTime, arrivalTime, seatsCount, price, status);
}