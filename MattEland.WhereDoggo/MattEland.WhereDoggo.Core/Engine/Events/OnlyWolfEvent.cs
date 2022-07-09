namespace MattEland.WhereDoggo.Core.Engine.Events;

public class OnlyWolfEvent : GameEventBase
{
    public OnlyWolfEvent(GamePlayer player) 
        : base(GamePhase.Night, player)
    {
        
    }

    public override string ToString()
    {
        return $"{Player} saw that they were the only doggo";
    }
}