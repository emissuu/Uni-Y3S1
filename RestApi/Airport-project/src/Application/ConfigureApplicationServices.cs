using Application.Flights.Services.Abstract;
using Application.Flights.Services.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IFlightService, FlightService>();
    }
}