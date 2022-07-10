namespace MattEland.WhereDoggo.Core.Strategies;

/// <summary>
/// A slot selection strategy that selects a random slot from the available slots, omitting a specific player.
/// </summary>
public class RandomNotSelfSlotSelectionStrategy : SlotSelectionStrategyBase
{
    private readonly Random _random;
    private readonly GamePlayer _self;

    public RandomNotSelfSlotSelectionStrategy(Random random, GamePlayer self)
    {
        _random = random;
        _self = self;
    }

    public override RoleContainerBase? SelectCard(IEnumerable<RoleContainerBase> slots)
        => slots.Where(s => s != _self).ToList().GetRandomElement(_random)!;
}