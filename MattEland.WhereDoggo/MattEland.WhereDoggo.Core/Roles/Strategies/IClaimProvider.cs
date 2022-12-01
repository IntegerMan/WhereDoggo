namespace MattEland.WhereDoggo.Core.Roles.Strategies;

public interface IClaimProvider
{
    IEnumerable<ClaimBase> GenerateClaims(Game game, GamePlayer player);
}