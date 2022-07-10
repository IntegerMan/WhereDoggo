namespace MattEland.WhereDoggo.Core.Engine;

/// <summary>
/// Represents a single game of One Night Ultimate Werewolf
/// </summary>
public class Game
{
    private List<GamePlayer> _players = new();
    private List<GameRoleBase> _roles = new();
    private readonly List<GameEventBase> _events = new();
    private List<RoleContainerBase> _roleContainers = new();

    public IList<GamePlayer> Players => _players.AsReadOnly();
    public IList<GameRoleBase> Roles => _roles.AsReadOnly();
    public IList<RoleSlot> CenterSlots => _centerSlots.AsReadOnly();
    public IList<RoleContainerBase> Entities => _roleContainers.AsReadOnly();
    public IList<GameEventBase> Events => _events.AsReadOnly();

    public Game(int numPlayers)
    {
        this.NumPlayers = numPlayers;
    }

    public Game(ICollection<RoleTypes> roles, bool randomizeSlots = true)
    {
        this.NumPlayers = roles.Count - NumCenterCards;
        SetUp(roles, randomizeSlots);
    }

    public void SetUp(IList<GameRoleBase> roles, bool randomizeSlots = true)
    {
        _roles = roles.ToList();
        string[] playerNames = { "Alice", "Bob", "Rufus", "Jimothy", "Wonko the Sane" };

        _roleContainers = new List<RoleContainerBase>(NumPlayers + NumCenterCards);

        if (randomizeSlots)
        {
            _roles = _roles.OrderBy(r => _random.Next() * _random.Next()).ToList();
        }

        int centerIndex = 1;
        for (int i = 0; i < roles.Count; i++)
        {
            if (i < NumPlayers)
            {
                _roleContainers.Add(new GamePlayer(playerNames[i], roles[i], this, _random));
            }
            else
            {
                RoleSlot slot = new($"Center Card {centerIndex++}", roles[i]);
                _roleContainers.Add(slot);
                _centerSlots.Add(slot);
            }
        }

        _players = _roleContainers.OfType<GamePlayer>().ToList();
    }

    public void SetUp(IEnumerable<RoleTypes> roles, bool randomizeSlots = true)
    {
        List<GameRoleBase> gameRoles = roles.Select(r => r.BuildGameRole()).ToList();
        SetUp(gameRoles, randomizeSlots);
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

        foreach (GamePlayer player in this._players)
        {
            LogEvent(new DealtRoleEvent(player, player.InitialRole));
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

        // TODO: This should really be a list of phases we cycle through
        WakeSentinels();
        WakeWerewolves();
        WakeInsomniac();
    }

    private void WakeSentinels()
    {
        List<GamePlayer> players = GetPlayersOfInitialRole(RoleTypes.Sentinel);
        foreach (GamePlayer player in players)
        {
            player.Wake();

            // Sentinels may choose to skip placing their token
            if (player.Strategies.SentinelTokenPlacementStrategy.SelectSlot(_players) is GamePlayer target)
            {
                if (target.InitialRole.RoleType == RoleTypes.Sentinel)
                {
                    throw new InvalidOperationException($"{player} attempted to place a sentinel token on themselves");
                }

                target.HasSentinelToken = true;
                LogEvent(new SentinelTokenPlacedEvent(player, target));
                LogEvent(new SentinelTokenObservedEvent(player, target, CurrentPhase));
            }
            else
            {
                LogEvent(new SentinelSkippedTokenPlacementEvent(player));
            }
        }
    }

    private List<GamePlayer> GetPlayersOfInitialRole(RoleTypes roleTypes)
        => Players.Where(p => p.InitialRole.RoleType == roleTypes).ToList();

    private void WakeInsomniac()
    {
        List<GamePlayer> players = Players.Where(p => p.InitialRole.RoleType == RoleTypes.Insomniac).ToList();
        foreach (GamePlayer player in players)
        {
            player.Wake();
            LogEvent(new InsomniacSawOwnCardEvent(player));
        }
    }

    private void WakeWerewolves()
    {
        List<GamePlayer> wolves = Players.Where(p => p.InitialTeam == Teams.Werewolves).ToList();
        wolves.ForEach(w => w.Wake());

        switch (wolves.Count)
        {
            case 0:
                LogEvent("No werewolves awoke");
                break;

            case 1:
                HandleLoneWolfWakes(wolves);
                break;

            case > 1:
                // Each wolf knows each other wolf is on team werewolf
                HandleMultipleWolvesWake(wolves);
                break;
        }
    }

    private void HandleLoneWolfWakes(IEnumerable<GamePlayer> doggos)
    {
        GamePlayer wolf = doggos.Single();
        LogEvent(new OnlyWolfEvent(wolf));
        foreach (GamePlayer otherPlayer in Players.Where(p => p != wolf))
        {
            LogEvent(new SawNotWerewolfEvent(wolf, otherPlayer));
        }

        RoleContainerBase? slot = wolf.Strategies.LoneWolfCenterCardStrategy.SelectSlot(_centerSlots);

        if (slot == null)
        {
            throw new InvalidOperationException("A lone werewolf did not pick a center card to look at");
        }

        LogEvent(new LoneWolfObservedCenterCardEvent(wolf, slot, slot.CurrentRole));
    }

    private void HandleMultipleWolvesWake(List<GamePlayer> doggos)
    {
        foreach (GamePlayer player in doggos)
        {
            foreach (GamePlayer otherPlayer in Players.Where(otherPlayer => otherPlayer != player))
            {
                if (otherPlayer.InitialTeam == Teams.Werewolves)
                {
                    LogEvent(new KnowsRoleEvent(CurrentPhase, player, otherPlayer, otherPlayer.CurrentRole));
                }
                else
                {
                    LogEvent(new SawNotWerewolfEvent(player, otherPlayer));
                }
            }
        }
    }

    public List<GameEventBase> FindEventsForPhase(GamePhase phase) =>
        Events.Where(e => e.Phase == phase)
            .OrderBy(e => e.Id)
            .ToList();

}