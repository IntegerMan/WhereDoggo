namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// Represents an instance of a specific role in the game. This is the base class for all roles.
/// It is possible for multiple instances of the same role to exist if they are in the game multiple times.
/// </summary>
public abstract class RoleBase
{
    /// <inheritdoc />
    public override string ToString() => RoleType.GetFriendlyName();
    
    /// <summary>
    /// The <see cref="RoleTypes"/> associated with the role instance
    /// </summary>
    public abstract RoleTypes RoleType { get; }

    /// <summary>
    /// The team the role is on
    /// </summary>
    public abstract Teams Team { get; }

    /// <summary>
    /// Determines whether or not the role has a night action
    /// </summary>
    public bool HasNightAction => NightActionOrder.HasValue;

    /// <summary>
    /// The order in which the night action of the role occurs. Lower numbers will go sooner.
    /// </summary>
    public virtual decimal? NightActionOrder => null;


    /// <summary>
    /// Handles the night action of the particular role. This should be overriden for roles with a night action
    /// </summary>
    /// <param name="game">The game instance.</param>
    /// <param name="player">The player woken up.</param>
    public virtual void PerformNightAction(Game game, GamePlayer player)
    {
    }
}