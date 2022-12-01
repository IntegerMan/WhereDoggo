namespace MattEland.WhereDoggo.Core.Events.Claims;

public class InsomniacClaim : ClaimBase
{
    public RoleTypes Role { get; }

    public InsomniacClaim(GamePlayer player, RoleTypes observedRole = RoleTypes.Insomniac) : base(player)
    {
        Role = observedRole;
    }

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, 
        IHasCard target, 
        CardProbabilities probabilities)
    {
        // Not sure what to do here. It's going to depend on the observer's trust of the player
    }

    /// <inheritdoc />
    public override string Text => Role == RoleTypes.Insomniac
        ? $"{Player} claims to have seen themselves as still the {RoleTypes.Insomniac.GetFriendlyName()}"
        : $"{Player} claims they were the {RoleTypes.Insomniac.GetFriendlyName()} but are now the {Role.GetFriendlyName()}";
}