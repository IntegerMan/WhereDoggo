using MattEland.WhereDoggo.Core.Roles;

namespace MattEland.WhereDoggo.Core.Gamespace;

/// <summary>
/// Represents a non-playable entity containing a game role only.
/// This is used for storing cards that may be swapped, but do not have a
/// player associated with them.
/// </summary>
public class RoleSlot : RoleContainerBase
{
    public RoleSlot(string name, GameRoleBase initialRole) : base(name, initialRole)
    {
    }
}