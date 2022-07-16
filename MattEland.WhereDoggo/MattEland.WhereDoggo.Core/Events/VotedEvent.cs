﻿namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// Represents a vote by one player on a specific target.
/// </summary>
public class VotedEvent : TargetedEventBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="VotedEvent"/> class.
    /// </summary>
    /// <param name="player">The player voting</param>
    /// <param name="target">The player they voted for</param>
    public VotedEvent(GamePlayer player, GamePlayer target) : base(GamePhase.Voting, player, target)
    {
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} voted for {Target}";
    
    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, CardContainer target, CardProbabilities probabilities)
    {
        // Do nothing
    }

}