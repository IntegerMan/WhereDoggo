using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// This event occurs when a player is observed by another player and now knows their role.
/// </summary>
public class KnowsRoleEvent : TargetedEventBase
{

    /// <summary>
    /// The role that the observed player is known to have.
    /// </summary>
    public RoleBase ObservedRole { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KnowsRoleEvent"/> class.
    /// </summary>
    /// <param name="phase">The phase of the game</param>
    /// <param name="observingPlayer">The player observing the other player</param>
    /// <param name="observedPlayer">The player being observed</param>
    public KnowsRoleEvent(GamePhases phase, GamePlayer observingPlayer, CardContainer observedPlayer) 
        : base(phase, observingPlayer, observedPlayer)
    {
        ObservedRole = observedPlayer.CurrentRole;
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} saw that {Target} is a {ObservedRole}";

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, CardContainer target, CardProbabilities probabilities)
    {
        if (target == Target)
        {
            probabilities.MarkAsCertainOfRole(ObservedRole.RoleType);
        }
    }
}