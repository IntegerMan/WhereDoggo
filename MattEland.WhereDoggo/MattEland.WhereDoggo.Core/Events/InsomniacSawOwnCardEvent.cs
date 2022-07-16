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
        FinalRole = player.CurrentRole;
    }

    /// <summary>
    /// Gets the final role that the Insomniac observed for their own card.
    /// </summary>
    public RoleBase FinalRole { get; }

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, CardContainer target, CardProbabilities probabilities)
    {
        if (observer == target)
        {
            probabilities.MarkAsCertainOfRole(target.CurrentRole.RoleType);
        }
    }

    /// <inheritdoc />
    public override string ToString() =>
        FinalRole.RoleType == Player!.InitialRole.RoleType 
            ? $"{Player} woke up and saw that they were still {FinalRole.RoleType}" 
            : $"{Player} woke up and saw that they had become a {FinalRole.RoleType}";
}