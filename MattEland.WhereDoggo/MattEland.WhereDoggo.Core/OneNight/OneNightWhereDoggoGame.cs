namespace MattEland.WhereDoggo.Core.OneNight;

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

    private readonly Random _random = new();
    private readonly List<RoleSlot> _centerSlots = new(NumCenterCards);

    public const int NumCenterCards = 3;

    public string Name => "One Night Ultimate Where Doggo?";

    public void LoadRoleContainers()
    {
        List<GameRoleBase> roles = this.Roles.OrderBy(r => _random.Next() + _random.Next() + _random.Next()).ToList();

        SetUp(roles);
    }

    public void SetUp(IList<GameRoleBase> roles)
    {
        string[] playerNames = {"Alice", "Bob", "Rufus", "Jimothy", "Wonko the Sane"};

        _roleContainers = new(NumPlayers + NumCenterCards);

        int centerIndex = 1;
        for (int i = 0; i < roles.Count; i++)
        {
            if (i < NumPlayers)
            {
                _roleContainers.Add(new GamePlayer(playerNames[i], roles[i]));
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

    public void PerformNightPhase()
    {
        LogEvent("Night Phase Starting");
        CurrentPhase = GamePhase.Night;

        WakeDoggos();

        LogEvent("Night Phase Ending");
        CurrentPhase = GamePhase.Day;
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

        RoleSlot slot = _centerSlots.GetRandomElement(_random)!;
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