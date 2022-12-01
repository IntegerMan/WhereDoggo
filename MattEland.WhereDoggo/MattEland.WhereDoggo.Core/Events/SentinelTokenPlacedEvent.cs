namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// This event is generated for the sentinel only when they place a token.
/// </summary>
public class SentinelTokenPlacedEvent : TargetedEventBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="SentinelTokenPlacedEvent"/> class.
    /// </summary>
    /// <param name="player">The sentinel player</param>
    /// <param name="target">The player receiving the token</param>
    public SentinelTokenPlacedEvent(GamePlayer player, GamePlayer target) : base(player, target)
    {
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} placed a sentinel token on {Target}";
    
    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        // Do nothing
    }

    /// <inheritdoc />
    public override IEnumerable<ClaimBase> GenerateClaims()
    {
        yield return new SentinelTokenPlacedClaim(Player!, Target);
    }

}