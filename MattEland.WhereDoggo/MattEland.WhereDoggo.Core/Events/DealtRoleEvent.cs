using MattEland.WhereDoggo.Core.Events.Claims;

namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// Occurs at the beginning of the game and lets the player know what their role is.
/// </summary>
public class DealtRoleEvent : GameEventBase
{
    /// <summary>
    /// The role the player was dealt
    /// </summary>
    public CardBase Role { get; }

    /// <summary>
    /// Instantiates a new instance of the <see cref="DealtRoleEvent"/> class.
    /// </summary>
    /// <param name="player">The player receiving the role</param>
    /// <param name="role">The role receivedS</param>
    public DealtRoleEvent(GamePlayer player, CardBase role) : base(player)
    {
        Role = role;
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player!.Name} was dealt {Role}";
    

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        if (observer == target)
        {
            probabilities.MarkAsCertainOfRole(Role.RoleType);
        }
    }

    /// <inheritdoc />
    public override IEnumerable<ClaimBase> GenerateClaims()
    {
        // This may need to be reigned in for wolves
        yield return new ClaimedRoleEvent(Player!, Role.RoleType);
    }
}