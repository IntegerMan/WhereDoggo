namespace MattEland.WhereDoggo.Core.Roles.Strategies;

public class FakeRevealerClaimProvider : IClaimProvider
{
    /// <inheritdoc />
    public IEnumerable<ClaimBase> GenerateClaims(Game game, GamePlayer player)
    {
        CenterCardSlot? revealed = game.CenterSlots.FirstOrDefault(s => s.CurrentCard.IsRevealed);

        if (revealed == null)
        {
            yield return new SkippedNightActionClaim(player);
        }
        else
        {
            if (revealed.CurrentCard.Team == Teams.Villagers)
            {
                yield return new RevealedGoodRoleClaim(player, revealed, revealed.CurrentCard.RoleType);
            }
            else
            {
                // Claim that the card was their own role
                yield return new RevealedEvilRoleClaim(player, revealed, player.InitialCard.RoleType);
            }
        }
    }
}