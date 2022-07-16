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

    protected GamePhaseBase(Game game)
    {
        Game = game;
    }
    
    /// <summary>
    /// The phase of the game
    /// </summary>
    public abstract GamePhases Phase { get; }
    
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

    protected void LogEvent(GameEventBase @event) => Game.LogEvent(@event);
    protected void LogEvent(string @event) => Game.LogEvent(@event);
    protected void BroadcastEvent(GameEventBase @event) => Game.BroadcastEvent(@event);
    protected void BroadcastEvent(string @event) => Game.BroadcastEvent(@event);
}