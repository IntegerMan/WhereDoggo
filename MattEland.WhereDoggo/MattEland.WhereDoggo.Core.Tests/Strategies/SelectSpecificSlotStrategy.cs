namespace MattEland.WhereDoggo.Core.Tests.Strategies;

/// <summary>
/// A strategy for selecting a specific slot. This exists for testing purposes.
/// </summary>
public class SelectSpecificSlotPlacementStrategy : SlotSelectionStrategyBase
{
    /// <summary>
    /// The index of the card to select
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Instantiates a new instance of the <see cref="SelectSpecificSlotPlacementStrategy"/> class.
    /// </summary>
    /// <param name="index">The index of the card to select</param>
    public SelectSpecificSlotPlacementStrategy(int index = 0) => Index = index;

    /// <inheritdoc />
    public override CardContainer? SelectCard(IEnumerable<CardContainer> options) => options.ToList()[Index];
}
