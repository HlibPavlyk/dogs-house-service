using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DogsHouseService.Infrastructure.Extensions;

public static class DependencyContainer
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbEfConnection(configuration);
    }
}