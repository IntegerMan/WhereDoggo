using MattEland.WhereDoggo.Core.Gamespace;

namespace MattEland.WhereDoggo.Core.Strategies;

public class RandomLoneWolfSlotSelectionStrategy : LoneWolfCardSelectionStrategyBase
{
    private readonly Random _random;

    public RandomLoneWolfSlotSelectionStrategy(Random random)
    {
        this._random = random;
    }

    public override RoleSlot SelectSlot(List<RoleSlot> centerSlots)
    {
        return centerSlots.GetRandomElement(_random)!;
    }
}