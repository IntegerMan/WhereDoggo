namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// Represents a non-playable entity containing a game role only.
/// This is used for storing cards that may be swapped, but do not have a
/// player associated with them.
/// </summary>
public class CenterCardSlot : IHasCard
{
    /// <summary>
    /// Creates a new instance of a <see cref="CenterCardSlot"/> class.
    /// </summary>
    /// <param name="slotName">The name of the slot.</param>
    /// <param name="initialCard">The initial card present in this role</param>
    public CenterCardSlot(string slotName, CardBase initialCard)
    {
        Name = slotName;
        InitialCard = initialCard;
        CurrentCard = initialCard;
    }

    /// <summary>
    /// Gets the name of the slot
    /// </summary>
    public string Name { get; }

    /// <inheritdoc />
    public CardBase InitialCard { get; }
    
    /// <inheritdoc />
    public CardBase CurrentCard { get; set; }

    /// <inheritdoc />
    public override string ToString() => $"{Name}";
}