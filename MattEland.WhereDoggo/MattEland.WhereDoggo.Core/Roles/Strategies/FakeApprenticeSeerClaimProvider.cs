namespace MattEland.WhereDoggo.Core.Roles.Strategies;

public class FakeApprenticeSeerClaimProvider : IClaimProvider
{
    /// <inheritdoc />
    public IEnumerable<ClaimBase> GenerateClaims(Game game, GamePlayer player)
    {
        // TODO: Pick a center slot you have a high confidence in and a role you have confidence in for that slot (or your own role)
        yield return new SawCardClaim(player, game.CenterSlots.First(), player.InitialCard.RoleType);
    }
}