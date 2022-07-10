namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// This event is generated for the sentinel only when they place a token.
/// </summary>
public class SentinelTokenPlacedEvent : GameEventBase
{
    /// <summary>
    /// The player that received the token.
    /// </summary>
    public GamePlayer Target { get; }

    public SentinelTokenPlacedEvent(GamePlayer player, GamePlayer target) : base(GamePhase.Night, player)
    {
        Target = target;
    }

    public override string ToString()
    {
        return $"{Player} placed a sentinel token on {Target}";
    }
}