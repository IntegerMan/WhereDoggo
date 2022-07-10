namespace MattEland.WhereDoggo.Core.Events;

public class VotedEvent : GameEventBase
{
    public GamePlayer Target { get; }

    public VotedEvent(GamePlayer player, GamePlayer target) : base(GamePhase.Voting, player)
    {
        Target = target;
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} voted for {Target}";
}