namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// This event occurs every time a player wakes up.
/// </summary>
public class WokeUpEvent : GameEventBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="WokeUpEvent"/> class.
    /// </summary>
    /// <param name="player">The player that woke up</param>
    public WokeUpEvent(GamePlayer player) : base(player)
    {
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} woke up in the {Phase}.";
    
    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, CardContainer target, CardProbabilities probabilities)
    {
        // Do nothing
    }

}