namespace MattEland.WhereDoggo.Core.Engine.Phases;

/// <summary>
/// Represents a phase within a game of One Night Ultimate Werewolf.
/// </summary>
public abstract class GamePhaseBase
{
    private readonly Queue<Action> _actions = new();

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

    /// <summary>
    /// Gets or sets whether or not the phase has completed
    /// </summary>
    public bool IsFinished { get; private set; }

    /// <inheritdoc />
    public override string ToString() => $"{Name} Phase";

    /// <summary>
    /// Runs the game phase
    /// </summary>
    /// <param name="game">The current game</param>
    public virtual void Run(Game game)
    {
        while (!IsFinished)
        {
            RunNext(game);
        }
    }

    /// <summary>
    /// Runs the next event in the game phase
    /// </summary>
    /// <param name="game">The current game</param>
    /// <inheritdoc />
    public virtual void RunNext(Game game)
    {
        if (_actions.TryDequeue(out var action))
        {
            action();

            if (_actions.Count <= 0)
            {
                IsFinished = true;
            }
        }
    }


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

    /// <summary>
    /// Enqueues an action to take place sequentially in the phase
    /// </summary>
    /// <param name="action">The Action to execute</param>
    protected void EnqueueAction(Action action) => _actions.Enqueue(action);

    /// <summary>
    /// Initializes data needed by the phase for an in-progress game
    /// </summary>
    /// <param name="game">The game</param>
    protected internal virtual void Initialize(Game game)
    {
        // Do nothing by default is fine
    }
}