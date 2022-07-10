namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// This event is added to all players when they first see a sentinel token on a card.
/// </summary>
public class SentinelTokenObservedEvent : GameEventBase
{
    public GamePlayer Target { get; }

    public SentinelTokenObservedEvent(GamePlayer player, GamePlayer target, GamePhase phase) : base(phase, player)
    {
        Target = target;
    }

    public override string ToString() => $"{Player} saw a sentinel token on {Target}";
}