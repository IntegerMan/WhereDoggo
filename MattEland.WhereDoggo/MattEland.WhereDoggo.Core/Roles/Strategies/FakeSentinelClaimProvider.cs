namespace MattEland.WhereDoggo.Core.Roles.Strategies;

public class FakeSentinelClaimProvider : IClaimProvider
{
    /// <inheritdoc />
    public IEnumerable<ClaimBase> GenerateClaims(Game game, GamePlayer player)
    {
        GamePlayer? target = game.Players.FirstOrDefault(p => p.HasSentinelToken);
        if (target == null)
        {
            yield return new SkippedNightActionClaim(player);
        }
        else
        {
            yield return new SentinelTokenPlacedClaim(player, target);
        }
    }
}