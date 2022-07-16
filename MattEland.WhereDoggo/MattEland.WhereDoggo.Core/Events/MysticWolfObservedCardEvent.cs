namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// An event that occurs when the <see cref="MysticWolfRole"/> observes the card of another player in the night.
/// </summary>
public class MysticWolfObservedCardEvent : TargetedEventBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="MysticWolfObservedCardEvent"/> class.
    /// </summary>
    /// <param name="player">The Mystic Wolf</param>
    /// <param name="target">The player who the Mystic Wolf observed</param>
    public MysticWolfObservedCardEvent(GamePlayer player, CardContainer target) : base(GamePhase.Night, player, target)
    {
        ObservedRole = target.CurrentRole;
    }

    /// <summary>
    /// The role the looked at card held
    /// </summary>
    public RoleBase ObservedRole { get; set; }

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, CardContainer target, CardProbabilities probabilities)
    {
        if (Target == target)
        {
            probabilities.MarkAsCertainOfRole(target.CurrentRole.RoleType);
        }
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} looked at {Target}'s card and saw it was a {ObservedRole}";
}