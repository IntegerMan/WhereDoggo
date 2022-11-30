namespace MattEland.WhereDoggo.Core.Events.Claims;

public class TappedByPlayerClaim : ClaimBase
{
    public IHasCard Tapper { get; }

    public TappedByPlayerClaim(GamePlayer player, IHasCard tapper) : base(player)
    {
        Tapper = tapper;
    }

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer,
        IHasCard target,
        CardProbabilities probabilities)
    {
        // Not sure what to do here. It's going to depend on the observer's trust of the player
    }

    /// <inheritdoc />
    public override string Text => $"{Player} claims to have been tapped by {Tapper}";
}