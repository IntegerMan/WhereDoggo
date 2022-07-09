using MattEland.WhereDoggo.Core.Gamespace;

namespace MattEland.WhereDoggo.Core.Strategies;

public abstract class LoneWolfCardSelectionStrategyBase
{
    public abstract RoleSlot SelectSlot(List<RoleSlot> centerSlots);
}