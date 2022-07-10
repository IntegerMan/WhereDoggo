namespace MattEland.WhereDoggo.Core.Strategies;

/// <summary>
/// A slot selection strategy that selects a random slot from the available slots.
/// </summary>
public class RandomSlotSelectionStrategy : SlotSelectionStrategyBase
{
    private readonly Random _random;

    public RandomSlotSelectionStrategy(Random random)
    {
        _random = random;
    }

    public override RoleContainerBase? SelectSlot(IEnumerable<RoleContainerBase> slots) 
        => slots.ToList().GetRandomElement(_random)!;
}