using Infomatrix.Core.Application.Abstractions.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Infomatrix.Core.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddCqrs();

        return services;
    }

    private static IServiceCollection AddCqrs(
        this IServiceCollection services)
    {
        services.AddScoped<ISender, Dispatcher>();

        return services.Scan(scan => scan
            .FromAssembliesOf(typeof(AssemblyReference))
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }
}
