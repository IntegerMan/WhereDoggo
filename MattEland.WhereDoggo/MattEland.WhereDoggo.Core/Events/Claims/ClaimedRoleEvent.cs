namespace MattEland.WhereDoggo.Core.Events.Claims;

/// <summary>
/// Represents a player claiming to be of a specific role
/// </summary>
public class ClaimedRoleEvent : ClaimBase
{
    /// <summary>
    /// The role the player claimed.
    /// </summary>
    public RoleTypes ClaimedRole { get; }

    /// <inheritdoc />
    public override bool IsLie => Player != null && ClaimedRole != Player.InitialCard.RoleType;

    /// <summary>
    /// Initializes a new instance of the <see cref="ClaimedRoleEvent"/> class.
    /// </summary>
    /// <param name="player">The player making the claim.</param>
    /// <param name="claimedRole">The claimed role</param>
    public ClaimedRoleEvent(GamePlayer player, RoleTypes claimedRole) : base(player)
    {
        ClaimedRole = claimedRole;
    }

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        // Not sure if this is necessary...
    }

    public override string Text => $"{Player} claimed to be a {ClaimedRole.GetFriendlyName()}";
}