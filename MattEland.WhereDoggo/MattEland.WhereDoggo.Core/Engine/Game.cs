namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// Represents a single game of One Night Ultimate Werewolf
/// </summary>
public class Game
{
    private readonly List<GameEventBase> _events = new();
    private readonly List<RoleContainerBase> _roleContainers;
    private readonly List<GamePlayer> _players;
    private readonly List<GameRoleBase> _roles;

    public IList<GamePlayer> Players => _players.AsReadOnly();
    public IList<GameRoleBase> Roles => _roles.AsReadOnly();
    public IList<RoleSlot> CenterSlots => _centerSlots.AsReadOnly();
    public IList<RoleContainerBase> Entities => _roleContainers.AsReadOnly();
    public IList<GameEventBase> Events => _events.AsReadOnly();

    public Game(ICollection<RoleTypes> roles, bool randomizeSlots = true)
    {
        this.NumPlayers = roles.Count - NumCenterCards;

        _roles = roles.Select(r => r.BuildGameRole()).ToList();
        
        string[] playerNames = { "Alice", "Bob", "Rufus", "Jimothy", "Wonko the Sane" };

        _roleContainers = new List<RoleContainerBase>(NumPlayers + NumCenterCards);

        if (randomizeSlots)
        {
            _roles = _roles.OrderBy(r => _random.Next() * _random.Next()).ToList();
        }

        _players = InitializePlayersAndCenterCards(playerNames);
    }

    private List<GamePlayer> InitializePlayersAndCenterCards(IReadOnlyList<string> playerNames)
    {
        int centerIndex = 1;
        for (int i = 0; i < _roles.Count; i++)
        {
            if (i < NumPlayers)
            {
                _roleContainers.Add(new GamePlayer(playerNames[i], _roles[i], this, _random));
            }
            else
            {
                RoleSlot slot = new($"Center Card {centerIndex++}", _roles[i]);
                _roleContainers.Add(slot);
                _centerSlots.Add(slot);
            }
        }

        return _roleContainers.OfType<GamePlayer>().ToList();
    }

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

    public void Start()
    {
        if (CurrentPhase != GamePhase.Setup)
            throw new InvalidOperationException("Game must be in setup phase");

        LogEvent($"{Name} started");

        foreach (GamePlayer player in _players)
        {
            LogEvent(new DealtRoleEvent(player, player.InitialRole));
            
            player.Brain.BuildInitialRoleProbabilities();
        }

        LogEvent($"{Name} initialized");
    }

    public GamePhase CurrentPhase { get; private set; } = GamePhase.Setup;
    private int _nextEventId;

    internal void LogEvent(string message) => LogEvent(new TextEvent(CurrentPhase, message));

    internal void LogEvent(GameEventBase @event)
    {
        @event.Id = _nextEventId++;

        _events.Add(@event);

        // The player involved should know about this event
        @event.Player?.AddEvent(@event);
    }

    internal void BroadcastEvent(string message)
    {
        TextEvent @event = new(CurrentPhase, message);

        BroadcastEvent(@event);
    }

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

    private readonly Random _random = new();
    private readonly List<RoleSlot> _centerSlots = new(NumCenterCards);

    public const int NumCenterCards = 3;

    public string Name => "One Night Ultimate Werewolf";

    public void PerformDayPhase()
    {
        LogEvent("Day Phase Starting");
        CurrentPhase = GamePhase.Day;

        _players.ForEach(p => p.Wake());
    }

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
            GamePlayer votedPlayer = player.DetermineVoteTarget(this, _random);

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

        bool wwVoted = votedPlayers.Any(p => p.CurrentRole.RoleType == RoleTypes.Werewolf);
        Result = new GameResult(wwVoted, wwVoted ? villagers : wolves);

        BroadcastEvent(wwVoted
            ? "The village wins!"
            : "The werewolves win!");

        BroadcastEvent(Result.Winners.Any()
            ? $"The winners are {string.Join(", ", Result.Winners.Select(w => $"{w.Name} ({w.CurrentRole})"))}"
            : "No players won.");

        return Result;
    }

    public GameResult? Result { get; set; }

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