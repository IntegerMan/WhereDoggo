﻿using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// Represents a single game of One Night Ultimate Werewolf
/// </summary>
public class Game
{
    private readonly List<GameEventBase> _events = new();
    private readonly List<CardContainer> _roleContainers;
    private readonly List<GamePlayer> _players;
    private readonly List<RoleBase> _roles;
    private readonly List<GamePhaseBase> _phases;

    /// <summary>
    /// Gets the players in the game
    /// </summary>
    public IList<GamePlayer> Players => _players.AsReadOnly();
    
    /// <summary>
    /// Gets the roles that were specified for the game.
    /// If roles occur more than once, they will be duplicated in this list.
    /// </summary>
    public IEnumerable<RoleBase> Roles => _roles.AsReadOnly();
    
    /// <summary>
    /// Gets a list of cards in the center slots
    /// </summary>
    public IList<CenterCardSlot> CenterSlots => _centerSlots.AsReadOnly();
    
    /// <summary>
    /// Gets all players and center slots
    /// </summary>
    public IEnumerable<CardContainer> Entities => _roleContainers.AsReadOnly();
    
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
        
        _roleContainers = new List<CardContainer>(NumPlayers + NumCenterCards);

        if (Options.RandomizeSlots)
        {
            _roles = _roles.OrderBy(r => Randomizer.Next() * Randomizer.Next()).ToList();
        }

        _players = InitializePlayersAndCenterCards(Options.PlayerNames);

        _phases = new()
        {
            new SetupGamePhase(this),
            new NightPhase(this),
            new DayPhase(this),
            new VotingPhase(this),
        };
    }

    /// <summary>
    /// Gets the options this game was created with
    /// </summary>
    public GameOptions Options { get; }

    private List<GamePlayer> InitializePlayersAndCenterCards(IReadOnlyList<string> playerNames)
    {
        int centerIndex = 1;
        for (int i = 0; i < _roles.Count; i++)
        {
            if (i < NumPlayers)
            {
                _roleContainers.Add(new GamePlayer(playerNames[i], i + 1, _roles[i], this, Randomizer));
            }
            else
            {
                CenterCardSlot slot = new($"Center Card {centerIndex++}", _roles[i]);
                _roleContainers.Add(slot);
                _centerSlots.Add(slot);
            }
        }

        return _roleContainers.OfType<GamePlayer>().ToList();
    }

    /// <summary>
    /// Gets the number of players playing. This will be the number of cards minus the count of cards in the center
    /// </summary>
    public int NumPlayers { get; }

    /// <summary>
    /// Carries out all phases of a game and returns the result of the game
    /// </summary>
    /// <returns>The result of the game</returns>
    public GameResult Run()
    {
        Start();
        
        PerformNightPhase();
        PerformDayPhase();
        
        foreach (GamePhaseBase phase in _phases)
        {
            BroadcastEvent($"Starting {phase.Name} phase");
            phase.Run(this);
        }

        return Result;
    }

    /// <summary>
    /// Starts the game by assigning cards to players and the center slots.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///Thrown if the game is not in the setup phase.
    /// </exception>
    public void Start()
    {
        if (CurrentPhase != GamePhases.Setup) throw new InvalidOperationException("Game must be in setup phase");

        LogEvent($"{Name} started");

        foreach (GamePlayer player in _players)
        {
            LogEvent(new DealtRoleEvent(player, player.InitialRole));
            
            player.Brain.BuildInitialRoleProbabilities();
        }

        LogEvent($"{Name} initialized");
    }

    /// <summary>
    /// Tracks the current phase of the game.
    /// </summary>
    public GamePhases CurrentPhase { get; private set; } = GamePhases.Setup;
    private int _nextEventId;

    /// <summary>
    /// Logs a new <see cref="TextEvent"/> at the game level
    /// </summary>
    /// <param name="message">The message to log</param>
    internal void LogEvent(string message) => LogEvent(new TextEvent(CurrentPhase, message));

    /// <summary>
    /// Logs an event that occurred in the game
    /// </summary>
    /// <param name="event">The event that occurred</param>
    internal void LogEvent(GameEventBase @event)
    {
        @event.Id = _nextEventId++;

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
        TextEvent @event = new(CurrentPhase, message);

        BroadcastEvent(@event);
    }

    /// <summary>
    /// Broadcasts an event to all players
    /// </summary>
    /// <param name="event">The event to broadcast</param>
    internal void BroadcastEvent(GameEventBase @event)
    {
        @event.Id = _nextEventId++;

        _events.Add(@event);

        // All players need to know about this event
        foreach (GamePlayer player in Players)
        {
            player.LogEvent(@event);
        }
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
    /// Performs the day phase of the game where players wake up and look for events
    /// and discuss their roles and actions.
    /// </summary>
    public void PerformDayPhase()
    {
        LogEvent("Day Phase Starting");
        CurrentPhase = GamePhases.Day;

        _players.ForEach(p => p.Wake());
    }

    /// <summary>
    /// The result of the game. This will be null if the game is not over
    /// </summary>
    public GameResult? Result { get; internal set; }

    /// <summary>
    /// The randomizer associated with this instance
    /// </summary>
    public Random Randomizer { get; } = new();

    /// <summary>
    /// Performs the night phase where players wake up and perform their night actions
    /// </summary>
    public void PerformNightPhase()
    {
        LogEvent("Night Phase Starting");
        CurrentPhase = GamePhases.Night;

        foreach (GamePlayer player in Players.Where(p => p.InitialRole.HasNightAction).OrderBy(p => p.InitialRole.NightActionOrder))
        {
            player.Wake();
            player.InitialRole.PerformNightAction(this, player);
        }
    }

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
}