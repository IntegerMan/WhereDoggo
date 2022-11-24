﻿namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// An event indicating that a player knows for certain a target does not have a given role.
/// Used by the <see cref="WerewolfRole"/> and the <see cref="MasonRole"/>
/// </summary>
public class SawNotRoleEvent : TargetedEventBase
{
    /// <summary>
    /// The role that it is not possible for the target to have
    /// </summary>
    public RoleTypes ImpossibleRole { get; }

    /// <inheritdoc />
    public override bool IsDeductiveEvent => true;

    /// <summary>
    /// Initializes a new instance of the <see cref="SawNotRoleEvent"/>
    /// </summary>
    /// <param name="observer">The player making the deduction</param>
    /// <param name="target">The target that cannot be the role</param>
    /// <param name="impossibleRole">The role that is not possible for the target to have</param>
    public SawNotRoleEvent(GamePlayer observer, IHasCard target, RoleTypes impossibleRole) : base(observer, target)
    {
        ImpossibleRole = impossibleRole;
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} saw that {Target} is not a {ImpossibleRole.GetFriendlyName()}";

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        if (target == Target)
        {
            probabilities.MarkAsCannotBeRole(ImpossibleRole);
        }
    }
}