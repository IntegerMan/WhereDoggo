namespace MattEland.WhereDoggo.Core.Roles.Strategies;

public class FakeMasonClaimProvider : IClaimProvider
{
    /// <inheritdoc />
    public IEnumerable<ClaimBase> GenerateClaims(Game game, GamePlayer player)
    {
        // Strategy: If there's another WW, maybe claim we saw them as Mason
        yield return new OnlyMasonClaim(player); 
    }
}