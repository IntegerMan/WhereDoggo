using MattEland.WhereDoggo.Core.Engine.Events;

namespace MattEland.WhereDoggo.Core.Engine;

public abstract class GameBase
{
    private readonly List<GamePlayer> _players;
    private readonly List<GameRoleBase> _roles;
    private readonly List<GameEventBase> _events = new();
    private readonly List<RoleContainerBase> _roleContainers;

    public IList<GamePlayer> Players => _players.AsReadOnly();
    public IList<GameRoleBase> Roles => _roles.AsReadOnly();
    public IList<RoleContainerBase> Entities => _roleContainers.AsReadOnly();
    public IList<GameEventBase> Events => _events.AsReadOnly();
    public abstract string Name { get; }

    public abstract List<RoleContainerBase> LoadRoleContainers(int numPlayers);
    public abstract List<GameRoleBase> LoadRoles(int numPlayers);

    protected GameBase(int numPlayers)
    {
        this._roles = this.LoadRoles(numPlayers);
        this._roleContainers = this.LoadRoleContainers(numPlayers);
        this._players = _roleContainers.OfType<GamePlayer>().ToList();
    }

    public void Start()
    {
        LogEvent($"{Name} started");

        foreach (GamePlayer player in this._players)
        {
            LogEvent(new DealtRoleEvent(player, player.InitialRole));
        }

        LogEvent($"{Name} initialized");
    }

    protected void LogEvent(string message)
    {
        LogEvent(new TextEvent(message));
    }

    private int _nextEventId = 0;
    protected void LogEvent(GameEventBase @event)
    {
        @event.Id = _nextEventId++;

        _events.Add(@event);

        // The player involved should know about this event
        @event.Player?.AddEvent(@event);
    }
}