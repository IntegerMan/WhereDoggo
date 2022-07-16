namespace MattEland.WhereDoggo.Core.Engine.Phases;

/// <summary>
/// Represents a phase within a game of One Night Ultimate Werewolf.
/// </summary>
public abstract class GamePhaseBase
{
    /// <summary>
    /// The game being simulated.
    /// </summary>
    public Game Game { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GamePhaseBase"/> class.
    /// </summary>
    /// <param name="game">The game</param>
    protected GamePhaseBase(Game game) => Game = game;
    
    /// <summary>
    /// Gets the name of the phase.
    /// </summary>
    public abstract string Name { get; }

    /// <inheritdoc />
    public override string ToString() => $"{Name} Phase";

    /// <summary>
    /// Runs the game phase
    /// </summary>
    /// <param name="game">The current game</param>
    public abstract void Run(Game game);

    /// <summary>
    /// Logs a new game event.
    /// </summary>
    /// <param name="event">The event to log</param>
    protected void LogEvent(GameEventBase @event) => Game.LogEvent(@event);
    
    /// <summary>
    /// Logs a new game event.
    /// </summary>
    /// <param name="event">The event to log</param>
    protected void LogEvent(string @event) => Game.LogEvent(@event);
    
    /// <summary>
    /// Broadcasts a new game event.
    /// </summary>
    /// <param name="event">The event to log</param>
    protected void BroadcastEvent(GameEventBase @event) => Game.BroadcastEvent(@event);
    
    /// <summary>
    /// Broadcasts a new game event.
    /// </summary>
    /// <param name="event">The event to log</param>
    protected void BroadcastEvent(string @event) => Game.BroadcastEvent(@event);
}