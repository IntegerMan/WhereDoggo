namespace MattEland.WhereDoggo.Core.Events.Claims;

public class TappedPlayerClaim : ClaimBase
{
    public IHasCard Target { get; }

    public TappedPlayerClaim(GamePlayer player, IHasCard target) : base(player)
    {
        Target = target;
    }

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer,
        IHasCard target,
        CardProbabilities probabilities)
    {
        // Not sure what to do here. It's going to depend on the observer's trust of the player
    }

    /// <inheritdoc />
    public override string Text => $"{Player} claims to have tapped {Target}";
}