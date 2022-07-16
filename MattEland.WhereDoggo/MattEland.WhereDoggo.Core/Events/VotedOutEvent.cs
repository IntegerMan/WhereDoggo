namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// This event occurs on the player(s) that get the most votes.
/// </summary>
public class VotedOutEvent : GameEventBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="VotedOutEvent"/> class.
    /// </summary>
    /// <param name="votedPlayer">The player that was voted out</param>
    public VotedOutEvent(GamePlayer votedPlayer) : base(votedPlayer)
    {

    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} was voted out. Their role was {Player!.CurrentCard}";
    
    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        // Do nothing
    }

}