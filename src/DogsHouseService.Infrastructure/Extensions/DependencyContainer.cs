using DogsHouseService.Application.Abstractions.Repositories;
using DogsHouseService.Application.Abstractions.Services;
using DogsHouseService.Application.Services;
using DogsHouseService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DogsHouseService.Infrastructure.Extensions;

public static class DependencyContainer
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbEfConnection(configuration);
        
        services.AddScoped<IDogRepository, DogRepository>();
        services.AddScoped<IDogService, DogService>();
    }
}