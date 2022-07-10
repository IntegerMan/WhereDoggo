namespace MattEland.WhereDoggo.Core.Strategies;

public abstract class SlotSelectionStrategyBase
{
    public abstract RoleContainerBase? SelectCard(IEnumerable<RoleContainerBase> slots);
}