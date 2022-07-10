namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// An event that occurs if a member of team Werewolf wakes up and sees that
/// there is no other werewolf in play.
/// </summary>
public class OnlyWolfEvent : GameEventBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OnlyWolfEvent"/> class.
    /// </summary>
    /// <param name="player">The player that is the lone wolf</param>
    public OnlyWolfEvent(GamePlayer player) : base(GamePhase.Night, player)
    {
        
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} saw that they were the only player on the werewolf team";
}