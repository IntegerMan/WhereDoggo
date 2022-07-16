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
                _roleContainers.Add(new GamePlayer(playerNames[i], _roles[i], this, Randomizer));
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
        
        return PerformVotePhase();
    }

    /// <summary>
    /// Starts the game by assigning cards to players and the center slots.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///Thrown if the game is not in the setup phase.
    /// </exception>
    public void Start()
    {
        if (CurrentPhase != GamePhase.Setup) throw new InvalidOperationException("Game must be in setup phase");

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
    public GamePhase CurrentPhase { get; private set; } = GamePhase.Setup;
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
        @event.Player?.AddEvent(@event);
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
            player.AddEvent(@event);
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
        CurrentPhase = GamePhase.Day;

        _players.ForEach(p => p.Wake());
    }

    /// <summary>
    /// Carries out the voting phase of the game and returns the result of the game.
    /// </summary>
    /// <returns>The result of the game</returns>
    public GameResult PerformVotePhase()
    {
        LogEvent("Voting Phase Starting");
        CurrentPhase = GamePhase.Voting;

        // Create a dictionary of votes without any votes in it
        Dictionary<GamePlayer, int> votes = new();
        foreach (GamePlayer player in Players)
        {
            votes[player] = 0;
        }

        // Get votes for individual players
        foreach (GamePlayer player in Players)
        {
            GamePlayer votedPlayer = player.DetermineVoteTarget(Randomizer);

            VotedEvent votedEvent = new(player, votedPlayer);
            LogEvent(votedEvent);

            votes[votedPlayer] += 1;
        }

        int maxVotes = votes.Values.Max();
        IEnumerable<GamePlayer> votedPlayers = votes.Where(kvp => kvp.Value == maxVotes).Select(kvp => kvp.Key);
        foreach (GamePlayer votedPlayer in votedPlayers)
        {
            BroadcastEvent(new VotedOutEvent(votedPlayer));
        }

        IEnumerable<GamePlayer> villagers = Players.Where(p => p.CurrentTeam == Teams.Villagers);
        IEnumerable<GamePlayer> wolves = Players.Where(p => p.CurrentTeam == Teams.Werewolves);

        bool wwVoted = votedPlayers.Any(p => p.CurrentTeam == Teams.Werewolves); // Revisit for Minion
        Result = new GameResult(wwVoted, wwVoted ? villagers : wolves);

        BroadcastEvent(wwVoted
            ? "The village wins!"
            : "The werewolves win!");

        BroadcastEvent(Result.Winners.Any()
            ? $"The winners are {string.Join(", ", Result.Winners.Select(w => $"{w.Name} ({w.CurrentRole})"))}"
            : "No players won.");

        return Result;
    }

    /// <summary>
    /// The result of the game. This will be null if the game is not over
    /// </summary>
    public GameResult? Result { get; private set; }

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
        CurrentPhase = GamePhase.Night;

        foreach (GamePlayer player in Players.Where(p => p.InitialRole.HasNightAction).OrderBy(p => p.InitialRole.NightActionOrder))
        {
            player.Wake();
            player.InitialRole.PerformNightAction(this, player);
        }
    }
}