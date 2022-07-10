namespace MattEland.WhereDoggo.Core.Strategies;

public class RandomSlotSelectionStrategy : SlotSelectionStrategyBase
{
    private readonly Random _random;

    public RandomSlotSelectionStrategy(Random random)
    {
        _random = random;
    }

    public override RoleContainerBase? SelectSlot(IEnumerable<RoleContainerBase> centerSlots) 
        => centerSlots.ToList().GetRandomElement(_random)!;
}