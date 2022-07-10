﻿namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// An abstract class for events targeting a specific card
/// </summary>
public abstract class TargetedEventBase : GameEventBase 
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="TargetedEventBase"/> class.
    /// </summary>
    /// <param name="phase">The phase the event occurred in</param>
    /// <param name="player"></param>
    /// <param name="target"></param>
    protected TargetedEventBase(GamePhase phase, GamePlayer player, RoleContainerBase target) : base(phase, player)
    {
        Target = target;
    }

    /// <summary>
    /// Gets the target of the event.
    /// </summary>
    public RoleContainerBase Target { get; }
}