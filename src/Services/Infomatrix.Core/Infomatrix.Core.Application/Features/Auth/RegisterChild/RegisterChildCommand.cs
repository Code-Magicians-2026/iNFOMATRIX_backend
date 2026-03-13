using Infomatrix.Core.Application.Abstractions.Messaging;
using Infomatrix.Core.Application.Abstractions.Repositories;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Application.DTOs.Child;
using Infomatrix.Core.Domain.Features.Child;
using Infomatrix.Core.Shared;

namespace Infomatrix.Core.Application.Features.Auth.RegisterChild;

public sealed record RegisterChildCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    Guid FamilyId)
    : ICommand<ChildDto>;

internal sealed class RegisterChildCommandHandler
    : ICommandHandler<RegisterChildCommand, ChildDto>
{
    private readonly IAuthService _authService;
    private readonly IRepository<ChildEntity> _childRepository;

    public RegisterChildCommandHandler(
        IAuthService authService,
        IRepository<ChildEntity> childRepository)
    {
        _authService = authService;
        _childRepository = childRepository;
    }

    public async Task<Result<ChildDto>> Handle(
        RegisterChildCommand command,
        CancellationToken cancellationToken)
    {
        var registerResult = await _authService.RegisterChildAsync(
            command.Email,
            command.FirstName,
            command.Password);

        if (registerResult.IsFailure)
        {
            return Result.Failure<ChildDto>(
                Error.Failure("Child.Error", "Child had not created"));
        }

        var child = ChildEntity.Create(
            registerResult.Value.Id,
            command.FirstName,
            command.LastName,
            command.Email,
            command.FamilyId);

        await _childRepository
            .AddAsync(child, cancellationToken);

        await _childRepository
            .SaveChangesAsync(cancellationToken);

        return Result.Success(new ChildDto(
            child.Id,
            child.FirstName,
            child.LastName,
            child.Experience));
    }
}