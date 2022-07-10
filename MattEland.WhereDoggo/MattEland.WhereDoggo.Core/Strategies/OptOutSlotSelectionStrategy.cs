namespace MattEland.WhereDoggo.Core.Strategies;

/// <summary>
/// A strategy that intentionally skips selecting a center card or player when given the choice on an optional ability.
/// </summary>
public class OptOutSlotSelectionStrategy : SlotSelectionStrategyBase
{
    public override RoleContainerBase? SelectCard(IEnumerable<RoleContainerBase> slots) => null;
}