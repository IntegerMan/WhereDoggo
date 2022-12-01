namespace MattEland.WhereDoggo.Core.Roles.Strategies;

public class FakeSeerClaimProvider : IClaimProvider
{
    /// <inheritdoc />
    public IEnumerable<ClaimBase> GenerateClaims(Game game, GamePlayer player)
    {
        // TODO: Pick a center slot you have a high confidence in and a role you have confidence in for that slot
        yield return new SawCardClaim(player, game.CenterSlots.First(), player.InitialCard.RoleType);

        // TODO: Pick a different center slot and different claim
        yield return new SawCardClaim(player, game.CenterSlots.Last(), RoleTypes.ApprenticeSeer);

        // Alternatively, pick a player and claim you looked at them and what their role is, but you have to have 
        // high confidence in their role and it has to be on the good team
    }
}