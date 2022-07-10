namespace MattEland.WhereDoggo.Core.Strategies;

public abstract class SlotSelectionStrategyBase
{
    public abstract RoleContainerBase? SelectSlot(IEnumerable<RoleContainerBase> slots);
}