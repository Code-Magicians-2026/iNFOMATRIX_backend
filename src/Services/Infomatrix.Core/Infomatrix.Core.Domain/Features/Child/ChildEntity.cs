using Infomatrix.Core.Domain.Common;
using Infomatrix.Core.Domain.Features.Family;
using Infomatrix.Core.Domain.Features.User;

namespace Infomatrix.Core.Domain.Features.Child;

public class ChildEntity : UserEntity
{
    public Guid FamilyId { get; private set; }
    public FamilyEntity Family { get; private set; } = null!;

    public int Experience { get; private set; }

    private ChildEntity()
    {
    }

    private ChildEntity(
        Guid id,
        string firstName,
        string lastName,
        string email,
        Guid familyId)
        : base(id, firstName, lastName, email)
    {
        FamilyId = familyId;
        Experience = 0;
    }

    public static ChildEntity Create(
        Guid id,
        string firstName,
        string lastName,
        string email,
        Guid familyId)
    {
        UserException.ThrowIfNameInvalid(firstName);
        UserException.ThrowIfNameInvalid(lastName);
        UserException.ThrowIfEmailInvalid(email);

        if (familyId == Guid.Empty)
            throw new DomainException("Child must be assigned to a family during creation");

        return new ChildEntity(id, firstName, lastName, email, familyId);
    }

    public void EarnPoints(int amount)
    {
        if (amount <= 0) return;
        Experience += amount;
    }

    public void SpendPoints(int amount)
    {
        if (amount > Experience)
            throw new DomainException("Insufficient balance to spend points");

        Experience -= amount;
    }
}