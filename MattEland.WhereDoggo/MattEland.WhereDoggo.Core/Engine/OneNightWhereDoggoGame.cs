namespace MattEland.WhereDoggo.Core.Engine;

public class OneNightWhereDoggoGame
{

    private List<GamePlayer> _players = new();
    private List<GameRoleBase> _roles = new();
    private readonly List<GameEventBase> _events = new();
    private List<RoleContainerBase> _roleContainers = new();

    public IList<GamePlayer> Players => _players.AsReadOnly();
    public IList<GameRoleBase> Roles => _roles.AsReadOnly();
    public IList<RoleContainerBase> Entities => _roleContainers.AsReadOnly();
    public IList<GameEventBase> Events => _events.AsReadOnly();

    public OneNightWhereDoggoGame(int numPlayers)
    {
        this.NumPlayers = numPlayers;
    }

    public void SetUp()
    {
        this.LoadRoles();
        this.LoadRoleContainers();
    }

    public void SetUp(IList<GameRoleBase> roles)
    {
        _roles = roles.ToList();
        string[] playerNames = { "Alice", "Bob", "Rufus", "Jimothy", "Wonko the Sane" };

        _roleContainers = new(NumPlayers + NumCenterCards);

        int centerIndex = 1;
        for (int i = 0; i < roles.Count; i++)
        {
            if (i < NumPlayers)
            {
                _roleContainers.Add(new GamePlayer(playerNames[i], roles[i], _random));
            }
            else
            {
                RoleSlot slot = new("Center Card " + (centerIndex++), roles[i]);
                _roleContainers.Add(slot);
                _centerSlots.Add(slot);
            }
        }

        _players = _roleContainers.OfType<GamePlayer>().ToList();
    }

    public int NumPlayers { get; }

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

    protected void LogEvent(string message)
    {
        LogEvent(new TextEvent(CurrentPhase, message));
    }

    public GamePhase CurrentPhase { get; protected set; } = GamePhase.Setup;
    private int _nextEventId = 0;
    protected void LogEvent(GameEventBase @event)
    {
        @event.Id = _nextEventId++;

        _events.Add(@event);

        // The player involved should know about this event
        @event.Player?.AddEvent(@event);
    }

    protected void BroadcastEvent(string message)
    {
        TextEvent @event = new(CurrentPhase, message);

        BroadcastEvent(@event);
    }

    protected void BroadcastEvent(GameEventBase @event)
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

    public string Name => "One Night Ultimate Where Doggo?";

    public void LoadRoleContainers()
    {
        List<GameRoleBase> roles = this.Roles.OrderBy(r => _random.Next() + _random.Next() + _random.Next()).ToList();

        SetUp(roles);
    }

    public List<GameRoleBase> LoadRoles()
    {
        _roles = new();

        const int numDoggos = 2;

        for (int i = 0; i < numDoggos; i++)
        {
            _roles.Add(new DoggoRole());
        }
        for (int i = 0; i < NumPlayers - numDoggos + NumCenterCards; i++)
        {
            _roles.Add(new RabbitRole());
        }

        return _roles;
    }


    public GameResult PerformDayPhase()
    {
        LogEvent("Day Phase Starting");
        CurrentPhase = GamePhase.Day;


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

        IEnumerable<GamePlayer> villagers = Players.Where(p => !p.CurrentRole.IsDoggo);
        IEnumerable<GamePlayer> wolves = Players.Where(p => p.CurrentRole.IsDoggo);

        bool wwVoted = votedPlayers.Any(p => p.CurrentRole.IsDoggo);
        Result = new GameResult
        {
            WerewolfKilled = wwVoted,
            Winners = wwVoted ? villagers : wolves
        };

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

        WakeDoggos();
    }

    private void WakeDoggos()
    {
        List<GamePlayer> doggos = Players.Where(p => p.InitialRole.IsDoggo).ToList();
        switch (doggos.Count)
        {
            case 0:
                LogEvent("No doggos awoke");
                break;

            case 1:
                HandleLoneDoggoWakes(doggos);
                break;

            case > 1:
                // Each doggo knows each other doggo is a doggo
                HandleMultipleDoggosWake(doggos);
                break;
        }
    }

    private void HandleLoneDoggoWakes(IEnumerable<GamePlayer> doggos)
    {
        GamePlayer loneDoggo = doggos.Single();
        LogEvent(new OnlyDoggoEvent(loneDoggo));
        foreach (GamePlayer otherPlayer in Players.Where(p => p != loneDoggo))
        {
            LogEvent(new SawNotDoggoEvent(loneDoggo, otherPlayer));
        }

        RoleSlot slot = loneDoggo.LoneWolfSlotSelectionStrategy.SelectSlot(_centerSlots);
        LogEvent(new LoneDoggoObservedCenterCardEvent(loneDoggo, slot, slot.CurrentRole));
    }

    private void HandleMultipleDoggosWake(List<GamePlayer> doggos)
    {
        foreach (GamePlayer player in doggos)
        {
            foreach (GamePlayer otherPlayer in Players.Where(otherPlayer => otherPlayer != player))
            {
                if (otherPlayer.StartedAsDoggo)
                {
                    LogEvent(new KnowsRoleEvent(CurrentPhase, player, otherPlayer, otherPlayer.CurrentRole));
                }
                else
                {
                    LogEvent(new SawNotDoggoEvent(player, otherPlayer));
                }
            }
        }
    }

    public List<GameEventBase> FindEventsForPhase(GamePhase phase) =>
        Events.Where(e => e.Phase == phase)
            .OrderBy(e => e.Id)
            .ToList();

}