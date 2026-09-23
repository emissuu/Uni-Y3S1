using Application.Common.Interfaces;
using Infrastructure.Persistance.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistance;

public static class ConfigurePersistanceServices
{
    public static void AddPersistanceServices(this IServiceCollection services)
    {
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddSingleton<IFlightRepository, FlightRepository>();
    }
}