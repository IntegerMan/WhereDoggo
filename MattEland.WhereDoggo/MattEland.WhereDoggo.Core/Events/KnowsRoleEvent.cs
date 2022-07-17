﻿namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// This event occurs when a player is observed by another player and now knows their role.
/// </summary>
public class KnowsRoleEvent : TargetedEventBase
{

    /// <summary>
    /// The role that the observed player is known to have.
    /// </summary>
    public CardBase ObservedCard { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KnowsRoleEvent"/> class.
    /// </summary>
    /// <param name="observingPlayer">The player observing the other player</param>
    /// <param name="observedPlayer">The player being observed</param>
    public KnowsRoleEvent( GamePlayer observingPlayer, IHasCard observedPlayer) 
        : base(observingPlayer, observedPlayer)
    {
        ObservedCard = observedPlayer.CurrentCard;
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} saw that {Target} is a {ObservedCard}";

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        if (target == Target)
        {
            probabilities.MarkAsCertainOfRole(ObservedCard.RoleType);
        }
    }
}