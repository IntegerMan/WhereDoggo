namespace MattEland.WhereDoggo.Core.Strategies;

/// <summary>
/// The base strategy for selecting a single card from a set of options
/// </summary>
public abstract class SlotSelectionStrategyBase
{
    /// <summary>
    /// Optionally selects a card from a list of options
    /// </summary>
    /// <remarks>
    /// This will be <c>null</c> for times when a player chose to skip their night ability or could not perform it.
    /// </remarks>
    /// <param name="options">The options of cards to select</param>
    /// <returns>The card that was selected, or null if this was skipped</returns>
    public abstract RoleContainerBase? SelectCard(IEnumerable<RoleContainerBase> options);
}