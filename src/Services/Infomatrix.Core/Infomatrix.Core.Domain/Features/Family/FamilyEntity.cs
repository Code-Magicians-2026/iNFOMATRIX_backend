using Infomatrix.Core.Domain.Common;
using Infomatrix.Core.Domain.Features.Child;
using Infomatrix.Core.Domain.Features.Parent;

namespace Infomatrix.Core.Domain.Features.Family;

public class FamilyEntity : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    public IReadOnlyCollection<ParentEntity> Parents => _parents.AsReadOnly();
    private readonly List<ParentEntity> _parents = new();

    public IReadOnlyCollection<ChildEntity> Children => _children.AsReadOnly();
    private readonly List<ChildEntity> _children = new();

    private FamilyEntity()
    {
    }

    private FamilyEntity(Guid id, string name)
        : base(id)
    {
        Name = name;
    }

    public static FamilyEntity Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Family name cannot be null or empty.", nameof(name));
        }

        return new FamilyEntity(Guid.NewGuid(), name);
    }

    public void AddParent(ParentEntity parent)
    {
        _parents.Add(parent);
    }

    public void AddChild(ChildEntity child)
    {
        _children.Add(child);
    }
}
