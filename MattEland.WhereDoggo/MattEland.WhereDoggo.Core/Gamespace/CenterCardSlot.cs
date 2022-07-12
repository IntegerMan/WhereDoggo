namespace MattEland.WhereDoggo.Core.Gamespace;

/// <summary>
/// Represents a non-playable entity containing a game role only.
/// This is used for storing cards that may be swapped, but do not have a
/// player associated with them.
/// </summary>
public class CenterCardSlot : CardContainer
{
    /// <summary>
    /// Creates a new instance of a <see cref="CenterCardSlot"/> class.
    /// </summary>
    /// <param name="name">The name of the slot.</param>
    /// <param name="initialRole">The initial card present in this role</param>
    public CenterCardSlot(string name, RoleBase initialRole) : base(name, initialRole)
    {
    }
}