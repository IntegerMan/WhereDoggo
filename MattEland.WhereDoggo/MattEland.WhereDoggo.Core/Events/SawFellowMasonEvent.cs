namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// This event occurs when a Mason is observed by another Mason and now knows their role.
/// </summary>
public class SawFellowMasonEvent : TargetedEventBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KnowsRoleEvent"/> class.
    /// </summary>
    /// <param name="observingPlayer">The player observing the other player</param>
    /// <param name="observedPlayer">The player being observed</param>
    public SawFellowMasonEvent( GamePlayer observingPlayer, IHasCard observedPlayer) 
        : base(observingPlayer, observedPlayer)
    {
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} saw that {Target} is a fellow {RoleTypes.Mason.GetFriendlyName()}";

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        if (target == Target)
        {
            probabilities.MarkAsCertainOfRole(RoleTypes.Mason);
        }
    }

    /// <inheritdoc />
    public override IEnumerable<ClaimBase> GenerateClaims()
    {
        yield return new SawFellowMasonClaim(Player!, Target);
    }
}