namespace MattEland.WhereDoggo.Core.OneNight;

public sealed class OneNightWhereDoggoGame : GameBase
{
    private readonly Random _random = new();
    private readonly List<RoleSlot> _centerSlots = new(NumCenterCards);

    public const int NumCenterCards = 3;

    public OneNightWhereDoggoGame(int numPlayers) : base(numPlayers)
    {
    }

    public override string Name => "One Night Ultimate Where Doggo?";

    public override List<RoleContainerBase> LoadRoleContainers(int numPlayers)
    {
        if (numPlayers is < 3 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(numPlayers), "Must have between 3 and 5 players");
        }

        if (Roles.Count < numPlayers)
        {
            throw new InvalidOperationException("Tried to load players but the number of players was less than the number of roles");
        }

        List<GameRoleBase> roles = this.Roles.OrderBy(r => _random.Next() + _random.Next() + _random.Next()).ToList();

        string[] playerNames = { "Alice", "Bob", "Rufus", "Jimothy", "Wonko the Sane" };

        List<RoleContainerBase> players = new(numPlayers + NumCenterCards);

        int centerIndex = 1;
        for (int i = 0; i < roles.Count; i++)
        {
            if (i < numPlayers)
            {
                players.Add(new GamePlayer(playerNames[i], roles[i]));
            }
            else
            {
                RoleSlot slot = new RoleSlot("Center Card " + (centerIndex++), roles[i]);
                players.Add(slot);
                _centerSlots.Add(slot);
            }
        }

        return players;
    }

    public override List<GameRoleBase> LoadRoles(int numPlayers)
    {
        if (numPlayers is < 3 or > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(numPlayers), "Must have between 3 and 5 players");
        }

        List<GameRoleBase> roles = new();

        const int numDoggos = 2;

        for (int i = 0; i < numDoggos; i++)
        {
            roles.Add(new DoggoRole());
        }
        for (int i = 0; i < numPlayers - numDoggos + NumCenterCards; i++)
        {
            roles.Add(new RabbitRole());
        }

        return roles;
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