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
    public ObservedPlayerCardEvent(GamePlayer player, IHasCard target) : base(player, target)
    {
        ObservedCard = target.CurrentCard;
    }

    /// <summary>
    /// The role the looked at card held
    /// </summary>
    public CardBase ObservedCard { get; set; }

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target, CardProbabilities probabilities)
    {
        if (Target == target)
        {
            probabilities.MarkAsCertainOfRole(target.CurrentCard.RoleType);
        }
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} looked at {Target}'s card and saw it was a {ObservedCard}";
}