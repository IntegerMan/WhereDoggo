namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// An event that occurs when a role gets to look at a card in the center.
/// This can currently happen to a lone <see cref="WerewolfRole"/> or an <see cref="ApprenticeSeerRole"/>
/// </summary>
public class ObservedCenterCardEvent : TargetedEventBase
{
    
    /// <summary>
    /// The role that was observed in that slot
    /// </summary>
    public RoleBase ObservedRole { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ObservedCenterCardEvent"/> class.
    /// </summary>
    /// <param name="player">The player observing the card</param>
    /// <param name="observedSlot">The card slot that was observed</param>
    /// <exception cref="ArgumentNullException">Thrown if the player was null</exception>
    public ObservedCenterCardEvent(GamePlayer player, CardContainer observedSlot) 
        : base(GamePhase.Night, player, observedSlot)
    {
        ObservedRole = observedSlot.CurrentRole;
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} saw {ObservedRole} in {Target}";

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, CardContainer target, CardProbabilities probabilities)
    {
        if (target == Target)
        {
            probabilities.MarkAsCertainOfRole(ObservedRole.RoleType);
        }
    }
}