using MattEland.WhereDoggo.Core.Gamespace;

namespace MattEland.WhereDoggo.Core.Events;

public class OnlyWolfEvent : GameEventBase
{
    public OnlyWolfEvent(GamePlayer player) 
        : base(GamePhase.Night, player)
    {
        
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} saw that they were the only player on the werewolf team";
}