namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// An abstract class for events targeting a specific card
/// </summary>
public abstract class TargetedEventBase : GameEventBase 
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="TargetedEventBase"/> class.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="target"></param>
    protected TargetedEventBase(GamePlayer player, CardContainer target) : base(player)
    {
        Target = target;
    }

    /// <summary>
    /// Gets the target of the event.
    /// </summary>
    public CardContainer Target { get; }
}