namespace MattEland.WhereDoggo.Core.Engine.Strategies;

public abstract class LoneWolfCardSelectionStrategyBase
{
    public abstract RoleSlot SelectSlot(List<RoleSlot> centerSlots);
}