namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// An event that occurs when the <see cref="MysticWolfRole"/> or <see cref="SeerRole"/> observes the card of another player in the night.
/// </summary>
public class ObservedPlayerCardEvent : TargetedEventBase
{
    /// <summary>
    /// Instantiates a new instance of the <see cref="ObservedPlayerCardEvent"/> class.
    /// </summary>
    /// <param name="player">The Mystic Wolf</param>
    /// <param name="target">The player who the Mystic Wolf observed</param>
    public ObservedPlayerCardEvent(GamePlayer player, CardContainer target) : base(player, target)
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