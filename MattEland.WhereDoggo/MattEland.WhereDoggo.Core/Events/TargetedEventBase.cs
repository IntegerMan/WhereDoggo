namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// An abstract class for events targeting a specific card
/// </summary>
public abstract class TargetedEventBase : GameEventBase 
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="TargetedEventBase"/> class.
    /// </summary>
    /// <param name="player">The player performing the action</param>
    /// <param name="target">The card holder targeted by the action</param>
    protected TargetedEventBase(GamePlayer player, IHasCard target) : base(player)
    {
        Target = target;
    }

    /// <summary>
    /// Gets the target of the event.
    /// </summary>
    public IHasCard Target { get; }
}