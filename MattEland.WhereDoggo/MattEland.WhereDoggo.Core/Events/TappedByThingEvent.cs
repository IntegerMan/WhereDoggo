namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// An event that occurs on a targeted player when a <see cref="ThingRole"/> uses its night ability to tap that player.
/// </summary>
public class TappedByThingEvent : TargetedEventBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="ThingTappedEvent"/> class.
    /// </summary>
    /// <param name="thing">The Thing player doing the tapping</param>
    /// <param name="target">The player that was tapped</param>
    public TappedByThingEvent(GamePlayer target, IHasCard thing) : base(target, thing)
    {
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} was tapped by {Target}";

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        // Certainties are handled in the tapped event
    }

    /// <inheritdoc />
    public override IEnumerable<ClaimBase> GenerateClaims()
    {
        yield return new TappedByPlayerClaim(Player!, Target);
    }

}