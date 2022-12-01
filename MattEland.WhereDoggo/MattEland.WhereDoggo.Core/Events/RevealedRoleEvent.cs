namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// Occurs when a <see cref="RevealerRole"/> or an <see cref="ExposerRole"/> reveals a card.
/// </summary>
public class RevealedRoleEvent : TargetedEventBase
{
    public RoleTypes Role { get; }

    /// <summary>
    /// Instantiates a new instance of the <see cref="RevealerHidEvilRoleEvent"/> class.
    /// </summary>
    /// <param name="player">The revealer</param>
    /// <param name="target">The card being hidden</param>
    public RevealedRoleEvent(GamePlayer player, IHasCard target, RoleTypes role) : base(player, target)
    {
        Role = role;
    }
    
    /// <inheritdoc />
    public override string ToString() => $"{Player} revealed {Target}'s role";
    
    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        // Do nothing
    }

    /// <inheritdoc />
    public override IEnumerable<ClaimBase> GenerateClaims()
    {
        if (Role.DetermineTeam() == Teams.Villagers)
        {
            yield return new RevealedGoodRoleClaim(Player!, Target, Role);
        }
        else
        {
            yield return new RevealedEvilRoleClaim(Player!, Target, Role);
        }
    }


}