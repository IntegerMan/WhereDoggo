namespace MattEland.WhereDoggo.Core.Strategies;

/// <summary>
/// A slot selection strategy that selects a random slot from the available slots, omitting a specific player.
/// </summary>
public class RandomNotSelfSlotSelectionStrategy : SlotSelectionStrategyBase
{
    private readonly Random _random;
    private readonly GamePlayer _avoidSelecting;

    /// <summary>
    /// Instantiates a new instance of the <see cref="RandomNotSelfSlotSelectionStrategy"/> class.
    /// </summary>
    /// <param name="random">The randomizer to use</param>
    /// <param name="avoidSelecting">The player to avoid selecting.</param>   
    public RandomNotSelfSlotSelectionStrategy(Random random, GamePlayer avoidSelecting)
    {
        _random = random;
        _avoidSelecting = avoidSelecting;
    }

    /// <inheritdoc />
    public override RoleContainerBase? SelectCard(IEnumerable<RoleContainerBase> options)
        => options.Where(s => s != _avoidSelecting).ToList().GetRandomElement(_random)!;
}