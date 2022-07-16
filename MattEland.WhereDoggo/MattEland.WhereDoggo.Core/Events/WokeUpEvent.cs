using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Events;

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
    public WokeUpEvent(GamePhases phase, GamePlayer player) : base(phase, player)
    {
    }

    /// <inheritdoc />
    public override string ToString() => Phase == GamePhases.Day 
        ? $"{Player} woke up in the morning." 
        : $"{Player} woke up in the {Phase}.";
    
    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, CardContainer target, CardProbabilities probabilities)
    {
        // Do nothing
    }

}