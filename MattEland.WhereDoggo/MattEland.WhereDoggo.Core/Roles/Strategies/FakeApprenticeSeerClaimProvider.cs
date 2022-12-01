namespace MattEland.WhereDoggo.Core.Roles.Strategies;

public class FakeApprenticeSeerClaimProvider : IClaimProvider
{
    /// <inheritdoc />
    public IEnumerable<ClaimBase> GenerateClaims(Game game, GamePlayer player)
    {
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Figure out the best center slot to claim
        decimal maxCertainty = game.CenterSlots.Max(s => probabilities[s].MaxCertainty);
        CenterCardSlot slot = game.CenterSlots.Where(s => probabilities[s].MaxCertainty == maxCertainty)
            .ToList()
            .GetRandomElement(game.Randomizer)!;

        // Get a random role that is likely to be acceptable from that slot
        RoleTypes claimedRole = probabilities[slot].PossibleRoles
            .Where(r => probabilities[slot].Probabilities[r] >= maxCertainty)
            .ToList()
            .GetRandomElement(game.Randomizer)!;

        // Do not shoot yourself in the foot or any known ally by claiming you saw their role claim in the center
        if (game.Players.Any(p =>
                probabilities[p].IsTeamCertain && probabilities[p].ProbableTeam == Teams.Werewolves &&
                p.ClaimedRole != null & p.ClaimedRole == claimedRole))
        {
            claimedRole = player.InitialCard.RoleType;
        }

        yield return new SawCardClaim(player, slot, claimedRole);
    }
}