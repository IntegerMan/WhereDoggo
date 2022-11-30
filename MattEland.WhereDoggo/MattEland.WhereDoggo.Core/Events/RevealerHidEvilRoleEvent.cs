using MattEland.WhereDoggo.Core.Events.Claims;

namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// Occurs when a revealer turns back over an evil card they previously revealed.
/// </summary>
public class RevealerHidEvilRoleEvent : TargetedEventBase
{
    private readonly RoleTypes _role;

    /// <summary>
    /// Instantiates a new instance of the <see cref="RevealerHidEvilRoleEvent"/> class.
    /// </summary>
    /// <param name="player">The revealer</param>
    /// <param name="target">The card being hidden</param>
    public RevealerHidEvilRoleEvent(GamePlayer player, IHasCard target) : base(player, target)
    {
        _role = target.CurrentCard.RoleType;
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} turned back over {Target}";
    
    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        // Do nothing
    }


    /// <inheritdoc />
    public override IEnumerable<ClaimBase> GenerateClaims()
    {
        yield return new RevealedEvilRoleClaim(Player!, Target, _role);
    }

}