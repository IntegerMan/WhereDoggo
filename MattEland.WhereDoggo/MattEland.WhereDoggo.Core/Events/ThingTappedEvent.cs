using MattEland.WhereDoggo.Core.Events.Claims;

namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// An event that occurs when a <see cref="ThingRole"/> uses its night ability to tap a player.
/// </summary>
public class ThingTappedEvent : TargetedEventBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="ThingTappedEvent"/> class.
    /// </summary>
    /// <param name="thing">The Thing player doing the tapping</param>
    /// <param name="target">The player that was tapped</param>
    public ThingTappedEvent(GamePlayer thing, IHasCard target) : base(thing, target)
    {
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} tapped {Target} as The Thing.";

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        // If a player is tapped, they should be certain that the tapper is the Thing.
        if (target == Player && observer == Target)
        {
            probabilities.MarkAsCertainOfRole(RoleTypes.Thing);
        }
    }

    /// <inheritdoc />
    public override IEnumerable<ClaimBase> GenerateClaims()
    {
        yield return new TappedPlayerClaim(Player!, Target);
    }

}