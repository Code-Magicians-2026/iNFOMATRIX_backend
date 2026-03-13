using Infomatrix.Core.Domain.Common;
using Infomatrix.Core.Domain.Features.Family;
using Infomatrix.Core.Domain.Features.User;

namespace Infomatrix.Core.Domain.Features.Parent;

public class ParentEntity : UserEntity
{
    public Guid? FamilyId { get; private set; } = default;

    public FamilyEntity Family { get; private set; } = null!;

    private ParentEntity()
    {
    }

    private ParentEntity(
        Guid id,
        string firstName,
        string lastName,
        string email)
        : base(id, firstName, lastName, email)
    {
    }

    public static ParentEntity Create(
        Guid id,
        string firstName,
        string lastName,
        string email)
    {
        UserException.ThrowIfNameInvalid(firstName);
        UserException.ThrowIfNameInvalid(lastName);
        UserException.ThrowIfEmailInvalid(email);

        return new ParentEntity(id, firstName, lastName, email);
    }

    public void JoinFamily(Guid familyId)
    {
        if (familyId == Guid.Empty)
            throw new DomainException("Invalid FamilyId");

        if (FamilyId.HasValue && FamilyId != familyId)
            throw new DomainException("User already has a family");

        FamilyId = familyId;
    }
}
