namespace MattEland.WhereDoggo.Core.Roles.Strategies;

public class FakeInsomniacClaimProvider : IClaimProvider
{
    /// <inheritdoc />
    public IEnumerable<ClaimBase> GenerateClaims(Game game, GamePlayer player)
    {
        yield return new InsomniacClaim(player);
    }
}