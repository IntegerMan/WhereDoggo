namespace MattEland.WhereDoggo.Core.Roles.Strategies;

public class FakeSeerClaimProvider : IClaimProvider
{
    /// <inheritdoc />
    public IEnumerable<ClaimBase> GenerateClaims(Game game, GamePlayer player)
    {
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        List<RoleTypes> forbiddenRoles = new();

        // Do not shoot yourself in the foot or any known ally by claiming you saw their role claim in the center
        foreach (GamePlayer p in game.Players.Where(p =>
                probabilities[p].IsTeamCertain && probabilities[p].ProbableTeam == Teams.Werewolves &&
                p.ClaimedRole != null))
        {
            forbiddenRoles.Add(p.ClaimedRole!.Value);
        }

        // Figure out the best center slot to claim
        IList<CenterCardSlot> slots = game.CenterSlots;

        SawCardClaim claim1 = ClaimCenterObservation(game, 
            player, 
            slots, 
            probabilities,
            forbiddenRoles);

        forbiddenRoles.Add(claim1.Role);

        SawCardClaim claim2 = ClaimCenterObservation(game, 
            player, slots.Where(s => s != claim1.Card).ToList(), probabilities, forbiddenRoles);

        yield return claim1;
        yield return claim2;

        // Alternatively, pick a player and claim you looked at them and what their role is, but you have to have 
        // high confidence in their role and it has to be on the good team
    }

    private static SawCardClaim ClaimCenterObservation(Game game,
        GamePlayer player,
        IList<CenterCardSlot> slots,
        IDictionary<IHasCard, CardProbabilities> probabilities,
        List<RoleTypes> forbiddenRoles)
    {
        decimal maxCertainty = slots.Max(s => probabilities[s].MaxCertainty);
        CenterCardSlot slot = slots.Where(s => probabilities[s].MaxCertainty == maxCertainty)
            .ToList()
            .GetRandomElement(game.Randomizer)!;

        // Get a random role that is likely to be acceptable from that slot
        RoleTypes claimedRole = probabilities[slot]
            .PossibleRoles
            .Where(r => !forbiddenRoles.Contains(r) && probabilities[slot].Probabilities[r] >= maxCertainty)
            .ToList()
            .GetRandomElement(game.Randomizer)!;

        return new SawCardClaim(player, slot, claimedRole);
    }
}