namespace MattEland.WhereDoggo.Core.Events.Claims;

public class SawFellowMasonClaim : ClaimBase
{
    public IHasCard Target { get; }

    public SawFellowMasonClaim(GamePlayer player, IHasCard target) : base(player)
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
    public override string Text => $"{Player} claims to have seen {Target} as a fellow {RoleTypes.Mason.GetFriendlyName()}";
}