namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// An event that occurs if the sentinel chose not to place their token.
/// Usually this is a bad idea, but it could work for some bluffing strategies.
/// This is currently applicable to the <see cref="SentinelRole"/>, <see cref="ApprenticeSeerRole"/>, and a lone <see cref="WerewolfRole"/>.
/// </summary>
public class SkippedNightActionEvent : GameEventBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SkippedNightActionEvent"/>
    /// </summary>
    /// <param name="player">The player who skipped their action</param>
    public SkippedNightActionEvent(GamePlayer player) : base(player)
    {
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} chose to skip their night action.";
    
    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, CardContainer target, CardProbabilities probabilities)
    {
        // Do nothing
    }

}