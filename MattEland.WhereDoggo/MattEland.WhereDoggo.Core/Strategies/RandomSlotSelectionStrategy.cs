namespace MattEland.WhereDoggo.Core.Strategies;

/// <summary>
/// A slot selection strategy that selects a random slot from the available slots.
/// </summary>
public class RandomSlotSelectionStrategy : SlotSelectionStrategyBase
{
    private readonly Random _random;

    /// <summary>
    /// Instantiates a new instance of the <see cref="RandomSlotSelectionStrategy"/> class.
    /// </summary>
    /// <param name="random">The randomizer to use</param>
    public RandomSlotSelectionStrategy(Random random) => _random = random;

    /// <inheritdoc />
    public override CardContainer? SelectCard(IEnumerable<CardContainer> options) 
        => options.ToList().GetRandomElement(_random)!;
}