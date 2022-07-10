﻿namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// This event occurs every time a player wakes up.
/// </summary>
public class WokeUpEvent : GameEventBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="WokeUpEvent"/> class.
    /// </summary>
    /// <param name="phase">The phase the event occurred. Can be night or day.</param>
    /// <param name="player">The player that woke up</param>
    public WokeUpEvent(GamePhase phase, GamePlayer player) : base(phase, player)
    {
    }

    /// <inheritdoc />
    public override string ToString() => Phase == GamePhase.Day 
        ? $"{Player} woke up in the morning." 
        : $"{Player} woke up in the {Phase}.";
}