using MattEland.WhereDoggo.Core.Roles;

namespace MattEland.WhereDoggo.Core.Gamespace;

public abstract class RoleContainerBase
{
    protected RoleContainerBase(string name, RoleBase initialRole)
    {
        Name = name;
        InitialRole = initialRole;
        CurrentRole = initialRole;
    }

    public string Name { get; }
    public RoleBase InitialRole { get; }
    public RoleBase CurrentRole { get; set; }

    public override string ToString() => Name;
}