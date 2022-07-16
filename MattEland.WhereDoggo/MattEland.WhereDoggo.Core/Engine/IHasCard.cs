namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// Marks that something has a card and allows tracking both the initial card and the current card.
/// </summary>
public interface IHasCard
{
    /// <summary>
    /// Represents the initial card dealt.
    /// </summary>
    CardBase InitialCard { get; }
    
    /// <summary>
    /// Represents the current card owned.
    /// </summary>
    CardBase CurrentCard { get; set;  }

    /// <summary>
    /// Gets the name of the slot
    /// </summary>
    string Name { get; }
}