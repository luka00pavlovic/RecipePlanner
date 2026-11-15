using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace RecipePlanner.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<IApplicationMarker>();
        return services;
    }
}

internal interface IApplicationMarker;