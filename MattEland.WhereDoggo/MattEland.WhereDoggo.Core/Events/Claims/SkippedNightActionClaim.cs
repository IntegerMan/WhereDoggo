namespace MattEland.WhereDoggo.Core.Events.Claims;

public class SkippedNightActionClaim : ClaimBase
{
    public SkippedNightActionClaim(GamePlayer player) : base(player)
    {

    }

    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        // Not sure what to do here. This likely should decrease trust in the player regardless of their role
    }

    /// <inheritdoc />
    public override string Text => $"{Player} claims to have skipped taking their night action";

}