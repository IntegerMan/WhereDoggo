namespace MattEland.WhereDoggo.Core.Events.Claims;

public class OnlyMasonClaim : ClaimBase
{
    public OnlyMasonClaim(GamePlayer player) : base(player)
    {
    }

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, 
        IHasCard target, 
        CardProbabilities probabilities)
    {
        // Not sure what to do here. It's going to depend on the observer's trust of the player
    }

    /// <inheritdoc />
    public override string Text => $"{Player} claims no other players woke as Masons";
}