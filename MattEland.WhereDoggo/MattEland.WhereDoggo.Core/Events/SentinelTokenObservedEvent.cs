namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// This event is added to all players when they first see a sentinel token on a card.
/// </summary>
public class SentinelTokenObservedEvent : TargetedEventBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="SentinelTokenObservedEvent"/> class.
    /// </summary>
    /// <param name="player">The player observing the token</param>
    /// <param name="target">The player that has the token</param>
    /// <param name="phase">The phase the token was first observed. Can be night or day.</param>
    public SentinelTokenObservedEvent(GamePlayer player, GamePlayer target, GamePhase phase) : base(phase, player, target)
    {
    }

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, CardContainer target, CardProbabilities probabilities)
    {
        // If we see a sentinel token, we know the sentinel cannot be in the center
        if (target is CenterCardSlot)
        {
            probabilities.MarkAsCannotBeRole(RoleTypes.Sentinel);
        }
    }

    /// <inheritdoc />
    public override string ToString() => Target == Player 
            ? $"{Player} saw a sentinel token on themselves" 
            : $"{Player} saw a sentinel token on {Target}";
}