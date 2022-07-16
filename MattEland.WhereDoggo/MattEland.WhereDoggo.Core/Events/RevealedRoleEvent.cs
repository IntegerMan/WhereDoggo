﻿namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// Occurs when a <see cref="RevealerRole"/> or an <see cref="ExposerRole"/> reveals a card.
/// </summary>
public class RevealedRoleEvent : TargetedEventBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="RevealerHidEvilRoleEvent"/> class.
    /// </summary>
    /// <param name="player">The revealer</param>
    /// <param name="target">The card being hidden</param>
    public RevealedRoleEvent(GamePlayer player, CardContainer target) : base(GamePhase.Night, player, target)
    {
    }
    
    /// <inheritdoc />
    public override string ToString() => $"{Player} revealed {Target}'s role";
    
    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, CardContainer target, CardProbabilities probabilities)
    {
        // Do nothing
    }

}