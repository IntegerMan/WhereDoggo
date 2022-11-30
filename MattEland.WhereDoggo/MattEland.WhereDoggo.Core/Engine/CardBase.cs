using MattEland.WhereDoggo.Core.Engine.Phases;
using MattEland.WhereDoggo.Core.Events.Claims;

namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// Represents an instance of a specific card in the game. This is the base class for all roles.
/// It is possible for multiple instances of the same role to exist if they are in the game multiple times.
/// </summary>
public abstract class CardBase
{
    public virtual IEnumerable<NightActionBase> NightActions => Enumerable.Empty<NightActionBase>();

    /// <inheritdoc />
    public override string ToString() => RoleType.GetFriendlyName();
    
    /// <summary>
    /// The <see cref="RoleTypes"/> associated with the role instance
    /// </summary>
    public abstract RoleTypes RoleType { get; }

    /// <summary>
    /// The team the role is on
    /// </summary>
    public abstract Teams Team { get; }
    
    /// <summary>
    /// Whether or not the card is revealed. Defaults to false but may be true if a <see cref="RevealerRole"/> or
    /// <see cref="ExposerRole"/>is present.
    /// </summary>
    public bool IsRevealed { get; set; }

    public IEnumerable<ClaimBase> GetClaims(GamePlayer player)
    {
        yield return new ClaimedRoleEvent(player, RoleType);

        foreach (ClaimBase? claim in GetClaimDetails(player))
        {
            yield return claim;
        }
    }

    protected virtual IEnumerable<ClaimBase> GetClaimDetails(GamePlayer player)
    {
        yield break;
    }
}