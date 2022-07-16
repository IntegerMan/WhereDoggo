﻿using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// Occurs when someone observes a role exposed by a <see cref="RevealerRole"/> or an <see cref="ExposerRole"/>
/// </summary>
public class RevealedRoleObservedEvent : KnowsRoleEvent
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="RevealedRoleObservedEvent"/> class.
    /// </summary>
    /// <param name="phase">The phase the event occurred</param>
    /// <param name="player">The player that observed the card.</param>
    /// <param name="target">The card being observed</param>
    public RevealedRoleObservedEvent(GamePhases phase, GamePlayer player, CardContainer target) : base(phase, player, target)
    {
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} saw that {Target} was revealed as {ObservedRole}";

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, CardContainer target, CardProbabilities probabilities)
    {
        base.UpdatePlayerPerceptions(observer, target, probabilities);

        RoleTypes inPlayRole = Target is CenterCardSlot 
            ? RoleTypes.Exposer 
            : RoleTypes.Revealer;
        
        probabilities.MarkRoleAsInPlay(inPlayRole);
    }
}