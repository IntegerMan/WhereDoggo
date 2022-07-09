using MattEland.WhereDoggo.Core.Gamespace;

namespace MattEland.WhereDoggo.Core.Strategies;

public class SelectSpecificSlotLoneWolfStrategy : LoneWolfCardSelectionStrategyBase
{
    public int Index { get; set; }

    public SelectSpecificSlotLoneWolfStrategy(int index = 0)
    {
        this.Index = index;
    }

    public override RoleSlot SelectSlot(List<RoleSlot> centerSlots)
    {
        return centerSlots[Index];
    }
}