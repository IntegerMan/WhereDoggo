﻿using MattEland.WhereDoggo.Core.Events;
using MattEland.WhereDoggo.Core.Gamespace;
using MattEland.WhereDoggo.Core.Roles;

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

    public void SetUp(IList<GameRoleBase> roles, bool randomizeSlots = true)
    {
        _roles = roles.ToList();
        string[] playerNames = { "Alice", "Bob", "Rufus", "Jimothy", "Wonko the Sane" };

        _roleContainers = new(NumPlayers + NumCenterCards);

        if (randomizeSlots)
        {
            _roles = _roles.OrderBy(r => _random.Next() * _random.Next()).ToList();
        }

        int centerIndex = 1;
        for (int i = 0; i < roles.Count; i++)
        {
            if (i < NumPlayers)
            {
                _roleContainers.Add(new GamePlayer(playerNames[i], roles[i], _random));
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

    public string Name => "One Night Ultimate Werewolf";

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

        IEnumerable<GamePlayer> villagers = Players.Where(p => p.CurrentTeam == Teams.Villagers);
        IEnumerable<GamePlayer> wolves = Players.Where(p =>  p.CurrentTeam == Teams.Werewolves);

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

        WakeWerewolves();
        WakeInsomniac();
    }

    private void WakeInsomniac()
    {
        List<GamePlayer> insomniacs = Players.Where(p => p.InitialRole.RoleType == RoleTypes.Insomniac).ToList();
        foreach (GamePlayer insomniac in insomniacs)
        {
            LogEvent(new InsomniacSawOwnCardEvent(insomniac));
        }
    }

    private void WakeWerewolves()
    {
        List<GamePlayer> wolves = Players.Where(p => p.InitialTeam == Teams.Werewolves).ToList();
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

        RoleSlot slot = wolf.LoneWolfSlotSelectionStrategy.SelectSlot(_centerSlots);
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