namespace MattEland.WhereDoggo.Core.Events.Claims;

public class RevealedEvilRoleClaim : ClaimBase
{
    public IHasCard Target { get; }
    public RoleTypes ObservedRole { get; }

    public RevealedEvilRoleClaim(GamePlayer player, IHasCard target, RoleTypes observedRole) : base(player)
    {
        Target = target;
        ObservedRole = observedRole;
    }

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer,
        IHasCard target,
        CardProbabilities probabilities)
    {
        // This should pretty easily be confirmable unless evils are claiming revealer too
    }

    /// <inheritdoc />
    public override string Text => $"{Player} claims to be the {Player.InitialCard.RoleType.GetFriendlyName()} that saw {Target} as {ObservedRole} and had to flip it back over";
}
