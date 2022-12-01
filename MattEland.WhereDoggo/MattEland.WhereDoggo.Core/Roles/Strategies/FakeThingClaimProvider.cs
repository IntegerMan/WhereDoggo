namespace MattEland.WhereDoggo.Core.Roles.Strategies;

public class FakeThingClaimProvider : IClaimProvider
{
    /// <inheritdoc />
    public IEnumerable<ClaimBase> GenerateClaims(Game game, GamePlayer player)
    {
        int prevIndex = game.GetPreviousPlayerIndex(player);
        int nextIndex = game.GetNextPlayerIndex(player);
        GamePlayer[] options = { game.Players[prevIndex], game.Players[nextIndex] };

        // If one of the two players is evil, say we tapped them and they'll likely cover for us
        // TODO: This is too much knowledge once we get to dream wolf and minion
        GamePlayer? target = options.FirstOrDefault(p => p.InitialCard.Team == player.InitialCard.Team);

        // If nobody near us was on our team, just pick a random person
        if (target == null)
        {
            target = options.GetRandomElement(game.Randomizer);
        }

        yield return new TappedPlayerClaim(player, target!);
    }
}