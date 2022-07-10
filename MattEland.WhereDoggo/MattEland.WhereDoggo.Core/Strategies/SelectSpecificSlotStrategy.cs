namespace MattEland.WhereDoggo.Core.Strategies;

public class SelectSpecificSlotPlacementStrategy : SlotSelectionStrategyBase
{
    public int Index { get; set; }

    public SelectSpecificSlotPlacementStrategy(int index = 0)
    {
        Index = index;
    }

    public override RoleContainerBase? SelectCard(IEnumerable<RoleContainerBase> slots) 
        => slots.ToList()[Index];
}
