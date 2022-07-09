namespace MattEland.WhereDoggo.Core.Events;

public class VotedEvent : GameEventBase
{
    public GamePlayer Target { get; }

    public VotedEvent(GamePlayer player, GamePlayer target) : base(GamePhase.Voting, player)
    {
        Target = target;
    }

    public override string ToString()
    {
        return $"{Player} voted for {Target}";
    }
}