namespace MattEland.WhereDoggo.Core.Roles.Strategies;

public class FakeMasonClaimProvider : IClaimProvider
{
    /// <inheritdoc />
    public IEnumerable<ClaimBase> GenerateClaims(Game game, GamePlayer player)
    {
        IDictionary<IHasCard, CardProbabilities> probabilities = player.Brain.BuildFinalRoleProbabilities();

        // Strategy: If there's another WW without a claim or claiming Mason, claim they're also a Mason
        foreach (GamePlayer otherPlayer in game.Players.Where(p => p != player))
        {
            if (probabilities[otherPlayer].IsTeamCertain &&
                probabilities[otherPlayer].ProbableTeam == Teams.Werewolves &&
                (otherPlayer.ClaimedRole is null or RoleTypes.Mason))
            {
                yield return new SawFellowMasonClaim(player, otherPlayer);
            }
        }

        yield return new OnlyMasonClaim(player); 
    }
}