﻿namespace MattEland.WhereDoggo.Core.Gamespace;

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
        LoneWolfCenterCardStrategy = new RandomSlotSelectionStrategy(random);
        SentinelTokenPlacementStrategy = new RandomSlotSelectionStrategy(random);
    }

    /// <summary>
    /// The strategy the lone wolf should use when they get to peek at a center card
    /// </summary>
    public SlotSelectionStrategyBase LoneWolfCenterCardStrategy { get; set; }

    /// <summary>
    /// The strategy the sentinel should use when they get to place their token
    /// </summary>
    public SlotSelectionStrategyBase SentinelTokenPlacementStrategy { get; set; } 
}