using Infomatrix.Core.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace Infomatrix.Core.Application.Abstractions.Messaging;

internal sealed class Dispatcher
    : ISender
{
    private readonly IServiceProvider _serviceProvider;

    public Dispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result> Send(
        ICommand command,
        CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<>)
            .MakeGenericType(command.GetType());
        var handler = _serviceProvider
            .GetRequiredService(handlerType);

        return await (Task<Result>)((dynamic)handler)
            .Handle((dynamic)command, cancellationToken);
    }

    public async Task<Result<TResponse>> Send<TResponse>(
        ICommand<TResponse> command,
        CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(ICommandHandler<,>)
            .MakeGenericType(command.GetType(), typeof(TResponse));
        var handler = _serviceProvider
            .GetRequiredService(handlerType);

        return await (Task<Result<TResponse>>)((dynamic)handler)
            .Handle((dynamic)command, cancellationToken);
    }

    public async Task<Result<TResponse>> Query<TResponse>(
        IQuery<TResponse> query,
        CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IQueryHandler<,>)
            .MakeGenericType(query.GetType(), typeof(TResponse));
        var handler = _serviceProvider
            .GetRequiredService(handlerType);

        return await (Task<Result<TResponse>>)((dynamic)handler)
            .Handle((dynamic)query, cancellationToken);
    }
}