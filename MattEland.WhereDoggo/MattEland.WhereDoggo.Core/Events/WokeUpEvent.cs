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
    /// <summary>
    ///  <inheritdoc />
    /// </summary>
    public override bool IsDeductiveEvent => Phase == "Day";

    /// <inheritdoc />
    public override string ToString()
    {
        if (Phase == "Day")
        {
            return $"{Player} woke up";
        }
        else
        { 
            return $"{Player} woke up in the {Phase} as the {Player!.InitialCard}";
        }
    }

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        // Do nothing
    }

}