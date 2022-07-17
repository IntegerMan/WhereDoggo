namespace MattEland.WhereDoggo.Core.Events.Claims;

/// <summary>
/// Represents a social claim during the day phase
/// </summary>
public abstract class ClaimBase : GameEventBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClaimBase"/> class.
    /// </summary>
    /// <param name="player">The player that made the claim</param>
    protected ClaimBase(GamePlayer player) : base(player)
    {
    }
}