namespace MattEland.WhereDoggo.Core.Engine.Events;

public class VotedOutEvent : GameEventBase
{
    public VotedOutEvent(GamePlayer votedPlayer) 
        : base(GamePhase.Voting, votedPlayer)
    {

    }

    public override string ToString() 
        => $"{Player} was voted out. Their role was {Player!.CurrentRole}";
}