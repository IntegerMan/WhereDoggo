using MattEland.WhereDoggo.Core.Events.Claims;

namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// Occurs when someone observes a role exposed by a <see cref="RevealerRole"/> or an <see cref="ExposerRole"/>
/// </summary>
public class RevealedRoleObservedEvent : KnowsRoleEvent
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="RevealedRoleObservedEvent"/> class.
    /// </summary>
    /// <param name="player">The player that observed the card.</param>
    /// <param name="target">The card being observed</param>
    public RevealedRoleObservedEvent( GamePlayer player, IHasCard target) : base(player, target)
    {
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} saw that {Target} was revealed as {ObservedCard}";

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        base.UpdatePlayerPerceptions(observer, target, probabilities);

        RoleTypes inPlayRole = Target is CenterCardSlot 
            ? RoleTypes.Exposer 
            : RoleTypes.Revealer;
        
        probabilities.MarkRoleAsInPlay(inPlayRole);
    }
}