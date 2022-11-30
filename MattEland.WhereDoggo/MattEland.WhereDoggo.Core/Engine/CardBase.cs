using MattEland.WhereDoggo.Core.Engine.Phases;
using MattEland.WhereDoggo.Core.Events.Claims;

namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// Represents an instance of a specific card in the game. This is the base class for all roles.
/// It is possible for multiple instances of the same role to exist if they are in the game multiple times.
/// </summary>
public abstract class CardBase
{
    /// <summary>
    /// Gets the night actions associated with this card.
    /// </summary>
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

    /// <summary>
    /// Gets all claims related to a player.
    /// This is used in discussion prior to voting.
    /// </summary>
    /// <param name="player">The player taking the action</param>
    /// <returns>Any claims</returns>
    public IEnumerable<ClaimBase> GetClaims(GamePlayer player)
    {
        foreach (GameEventBase observedEvent in player.OwnEvents.ToList())
        {
            foreach (ClaimBase claim in observedEvent.GenerateClaims())
            {
                yield return claim;
            }
        }
    }
}