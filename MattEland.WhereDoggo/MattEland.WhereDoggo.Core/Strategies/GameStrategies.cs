namespace MattEland.WhereDoggo.Core.Strategies;

/// <summary>
/// A container for game strategies for different events.
/// This is primarily customizable for consistent unit tests and testing specific scenarios.
/// </summary>
public class GameStrategies
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameStrategies"/> class.
    /// </summary>
    /// <param name="random">The randomizer</param>
    /// <param name="self">The player making the choices</param>
    public GameStrategies(Random random, GamePlayer self)
    {
        PickSingleCardStrategy = new RandomSlotSelectionStrategy(random);
        SentinelTokenPlacementStrategy = new RandomNotSelfSlotSelectionStrategy(random, self);
        PickSeerCards = (_, slots) => slots.OrderBy(_ => random.Next() * random.Next()).Take(2).ToList();
    }

    /// <summary>
    /// The strategy the lone wolf should use when they get to peek at a center card
    /// </summary>
    public SlotSelectionStrategyBase PickSingleCardStrategy { get; set; }

    /// <summary>
    /// The strategy the sentinel should use when they get to place their token
    /// </summary>
    public SlotSelectionStrategyBase SentinelTokenPlacementStrategy { get; set; }

    /// <summary>
    /// The function to use when picking cards for the <see cref="SeerRole"/>. A seer-specific function is required
    /// because the seer gets the choice to skip, pick one card from a player, or pick two cards from the center.
    /// </summary>
    public Func<IList<CardContainer>, IList<CardContainer>, List<CardContainer>> PickSeerCards { get; set; }
        
}