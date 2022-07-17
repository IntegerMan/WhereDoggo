namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// An event that occurs in the game, often pertaining to a single player.
/// </summary>
public abstract class GameEventBase
{
    /// <summary>
    /// The game phase the event occurred on
    /// </summary>
    public string? Phase { get; set; }
    
    /// <summary>
    /// The player the event occurred to. This may be null for some general events.
    /// </summary>
    public GamePlayer? Player { get; }

    /// <summary>
    /// The event's Id. This is a sequential number that is unique to the event.
    /// Events with a lower number occur before other events.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GameEventBase"/> class.
    /// </summary>
    /// <param name="player">The player the game event occurred to. Optional and may be null.</param>
    protected GameEventBase(GamePlayer? player = null)
    {
        Player = player;
    }

    /// <summary>
    /// Updates the player's probabilistic model of <paramref name="target"/> based on the event.
    /// </summary>
    /// <param name="observer">The <see cref="GamePlayer"/> privy to the event</param>
    /// <param name="target">The <see cref="IHasCard"/> the event occurred on</param>
    /// <param name="probabilities">The <see cref="CardProbabilities"/> for <paramref name="target" /></param>
    public abstract void UpdatePlayerPerceptions(GamePlayer observer, IHasCard target,
        CardProbabilities probabilities);
}