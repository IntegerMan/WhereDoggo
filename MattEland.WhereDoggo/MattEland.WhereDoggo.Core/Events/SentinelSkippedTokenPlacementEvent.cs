namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// An event that occurs if the sentinel chose not to place their token.
/// Usually this is a bad idea, but it could work for some bluffing strategies.
/// </summary>
public class SentinelSkippedTokenPlacementEvent : GameEventBase
{
    public SentinelSkippedTokenPlacementEvent(GamePlayer player) : base(GamePhase.Night, player)
    {
    }

    public override string ToString() => $"{Player} skipped placing their token.";
}