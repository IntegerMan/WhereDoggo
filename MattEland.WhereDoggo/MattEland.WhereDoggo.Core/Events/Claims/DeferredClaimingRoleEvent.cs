namespace MattEland.WhereDoggo.Core.Events.Claims;

/// <summary>
/// This event occurs when a player declines to claim a role. This is sometimes done early on in a game.
/// </summary>
public class DeferredClaimingRoleEvent : ClaimBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeferredClaimingRoleEvent"/> class.
    /// </summary>
    /// <param name="player">The player refusing to claim a role</param>
    public DeferredClaimingRoleEvent(GamePlayer player) : base(player)
    {
    }

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        // Does nothing at the moment
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} decided to wait before claiming their role.";
}
