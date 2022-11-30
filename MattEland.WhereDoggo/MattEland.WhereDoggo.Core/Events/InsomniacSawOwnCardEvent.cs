using MattEland.WhereDoggo.Core.Events.Claims;
using System.Data;

namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// This event occurs when the <see cref="InsomniacRole"/> looks at her own card at the end of the night.
/// </summary>
public class InsomniacSawOwnCardEvent : GameEventBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InsomniacSawOwnCardEvent"/> class.
    /// </summary>
    /// <param name="player">The insomniac player</param>
    public InsomniacSawOwnCardEvent(GamePlayer player) : base(player)
    {
        FinalCard = player.CurrentCard;
    }

    /// <summary>
    /// Gets the final role that the Insomniac observed for their own card.
    /// </summary>
    public CardBase FinalCard { get; }

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        if (observer == target)
        {
            probabilities.MarkAsCertainOfRole(target.CurrentCard.RoleType);
        }
    }

    /// <inheritdoc />
    public override IEnumerable<ClaimBase> GenerateClaims()
    {
        yield return new InsomniacClaim(Player!, FinalCard.RoleType);
    }

    /// <inheritdoc />
    public override string ToString() =>
        FinalCard.RoleType == Player!.CurrentCard.RoleType 
            ? $"{Player} woke up and saw that they were still {FinalCard.RoleType}" 
            : $"{Player} woke up and saw that they had become a {FinalCard.RoleType}";
}