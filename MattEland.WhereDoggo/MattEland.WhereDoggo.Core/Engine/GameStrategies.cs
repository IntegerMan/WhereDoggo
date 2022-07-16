namespace MattEland.WhereDoggo.Core.Engine;

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
    public GameStrategies(Random random)
    {
        PickSingleCard = (targets) => targets.MinBy(_ => random.Next() * random.Next());
        PickSeerCards = (_, slots) => slots.OrderBy(_ => random.Next() * random.Next()).Take(2).ToList();
    }

    /// <summary>
    /// The strategy to use when selecting a single card from multiple. Applies to multiple roles
    /// </summary>
    public Func<IEnumerable<CardContainer>, CardContainer?> PickSingleCard { get; set; }

    /// <summary>
    /// The function to use when picking cards for the <see cref="SeerRole"/>. A seer-specific function is required
    /// because the seer gets the choice to skip, pick one card from a player, or pick two cards from the center.
    /// </summary>
    public Func<IEnumerable<CardContainer>, IEnumerable<CardContainer>, List<CardContainer>> PickSeerCards { get; set; }
        
}