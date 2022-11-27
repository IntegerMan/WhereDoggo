using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// Represents a single game of One Night Ultimate Werewolf
/// </summary>
public class Game
{
    private readonly List<GameEventBase> _events = new();
    private readonly List<GamePlayer> _players = new();
    private readonly List<CardBase> _roles;
    private readonly Queue<GamePhaseBase> _phases;

    /// <summary>
    /// Gets the players in the game
    /// </summary>
    public IList<GamePlayer> Players => _players.AsReadOnly();
    
    /// <summary>
    /// Gets the roles that were specified for the game.
    /// If roles occur more than once, they will be duplicated in this list.
    /// </summary>
    public IEnumerable<CardBase> Roles => _roles.AsReadOnly();
    
    /// <summary>
    /// Gets a list of cards in the center slots
    /// </summary>
    public IList<CenterCardSlot> CenterSlots => _centerSlots.AsReadOnly();

    /// <summary>
    /// Gets all players and center slots
    /// </summary>
    public IEnumerable<IHasCard> Entities
    {
        get
        {
            List<IHasCard> entities = new();
            entities.AddRange(_players);
            entities.AddRange(_centerSlots);
            
            return entities;
        }
    }
    
    /// <summary>
    /// Gets all events that have occurred in the game, regardless of player they occurred to.
    /// </summary>
    public IEnumerable<GameEventBase> Events => _events.AsReadOnly();

    /// <summary>
    /// Instantiates a new game with the specified roles.
    /// </summary>
    /// <param name="roles">The roles to include in the game</param>
    /// <param name="options">Options related to launching the game</param>
    public Game(ICollection<RoleTypes> roles, GameOptions? options = null)
    {
        this.Options = options ?? new GameOptions();
        this.NumPlayers = roles.Count - NumCenterCards;

        _roles = roles.Select(r => r.BuildGameRole()).ToList();

        _phases = new Queue<GamePhaseBase>();
        _phases.Enqueue(new SetupGamePhase(this));
        _phases.Enqueue(new NightPhase(this));
        _phases.Enqueue(new DayPhase(this));
        _phases.Enqueue(new VotingPhase(this));
        CurrentPhase = _phases.Peek();
    }

    /// <summary>
    /// Gets the options this game was created with
    /// </summary>
    public GameOptions Options { get; }

    /// <summary>
    /// Gets the number of players playing. This will be the number of cards minus the count of cards in the center
    /// </summary>
    public int NumPlayers { get; }

    /// <summary>
    /// Carries out all phases of a game and returns the result of the game
    /// </summary>
    public void Run()
    {
        
        while (_phases.Any())
        {
            RunNextPhase();
        }
    }

    /// <summary>
    /// Runs the next phase of the game and returns whether or not the game is over
    /// </summary>
    /// <returns>Returns <c>true</c> if the game is over, otherwise <c>false</c>.</returns>
    public bool RunNextPhase()
    {
        GamePhaseBase phase = _phases.Dequeue();
        CurrentPhase = phase;
        BroadcastEvent($"Starting {phase.Name} phase");
        phase.Run(this);

        return Result != null;
    }

    /// <summary>
    /// Tracks the current phase of the game.
    /// </summary>
    public GamePhaseBase CurrentPhase { get; private set; }
    private int _nextEventId;

    /// <summary>
    /// Logs a new <see cref="TextEvent"/> at the game level
    /// </summary>
    /// <param name="message">The message to log</param>
    internal void LogEvent(string message) => LogEvent(new TextEvent(message));

    /// <summary>
    /// Logs an event that occurred in the game
    /// </summary>
    /// <param name="event">The event that occurred</param>
    internal void LogEvent(GameEventBase @event)
    {
        SetEventFields(@event);

        _events.Add(@event);

        // The player involved should know about this event
        @event.Player?.LogEvent(@event);
    }

    /// <summary>
    /// Broadcasts a message event to all players
    /// </summary>
    /// <param name="message">The message</param>
    internal void BroadcastEvent(string message)
    {
        TextEvent @event = new(message);

        BroadcastEvent(@event);
    }

    /// <summary>
    /// Broadcasts an event to all players
    /// </summary>
    /// <param name="event">The event to broadcast</param>
    internal void BroadcastEvent(GameEventBase @event)
    {
        SetEventFields(@event);

        _events.Add(@event);

        // All players need to know about this event
        foreach (GamePlayer player in Players)
        {
            player.LogEvent(@event);
        }
    }

    private void SetEventFields(GameEventBase @event)
    {
        if (@event.Id <= 0)
        {
            @event.Id = _nextEventId++;
        }
        
        @event.Phase = CurrentPhase.Name;
    }

    private readonly List<CenterCardSlot> _centerSlots = new(NumCenterCards);

    /// <summary>
    /// The number of cards in the center
    /// </summary>
    public const int NumCenterCards = 3;

    /// <summary>
    /// Gets the name of the game
    /// </summary>
    public string Name => Options.GameName;
    
    /// <summary>
    /// The result of the game. This will be null if the game is not over
    /// </summary>
    public GameResult? Result { get; internal set; }

    /// <summary>
    /// The randomizer associated with this instance
    /// </summary>
    public Random Randomizer { get; } = new();

    /// <summary>
    /// Gets a value indicating whether or not the game is currently over
    /// </summary>
    public bool IsCompleted => _phases.Count <= 0;

    /// <summary>
    /// Gets the index of the previous player. This is commonly used for adjacency abilities.
    /// </summary>
    /// <param name="player">The player</param>
    /// <returns>The zero-based index of the previous player</returns>
    public int GetPreviousPlayerIndex(GamePlayer player) => player.Number == 1 
            ? Players.Count - 1
            : player.Number - 2;
    
    /// <summary>
    /// Gets the index of the next player. This is commonly used for adjacency abilities.
    /// </summary>
    /// <param name="player">The player</param>
    /// <returns>The zero-based index of the next player</returns>
    public int GetNextPlayerIndex(GamePlayer player) => player.Number == Players.Count 
            ? 0
            : player.Number;

    internal void AddPlayer(GamePlayer gamePlayer) => _players.Add(gamePlayer);
    internal void AddCenterCard(CenterCardSlot slot) => _centerSlots.Add(slot);

    /// <summary>
    /// Gets a list of targets aside from the player.
    /// </summary>
    /// <param name="player">The player looking for targets</param>
    /// <param name="omitProtected">Whether or not to include sentinel tokens</param>
    /// <returns>Valid targets for the player</returns>
    public IEnumerable<IHasCard> GetOtherPlayerTargets(GamePlayer player, bool omitProtected = true) 
        => Players.Where(p => p != player && (!omitProtected || !p.HasSentinelToken));
}