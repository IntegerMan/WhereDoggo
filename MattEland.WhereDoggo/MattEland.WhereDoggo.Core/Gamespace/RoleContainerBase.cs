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

    /// <summary>
    /// Whether or not the card is revealed. Defaults to false but may be true if a <see cref="RevealerRole"/> is present.
    /// </summary>
    public bool IsRevealed { get; set; }

    public override string ToString() => Name;
}