using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipePlanner.Infrastructure.Persistence;

namespace RecipePlanner.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<RecipePlannerDbContext>(options => options.UseSqlite(connectionString));
        return services;
    }
}
