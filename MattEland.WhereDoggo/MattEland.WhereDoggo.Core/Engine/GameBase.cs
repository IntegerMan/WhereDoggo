namespace MattEland.WhereDoggo.Core.Engine;

public abstract class GameBase
{
    private readonly List<GamePlayer> _players;
    private readonly List<GameRoleBase> _roles;
    private readonly List<GameEventBase> _events = new();

    public IList<GamePlayer> Players => _players.AsReadOnly();
    public IList<GameRoleBase> Roles => _roles.AsReadOnly();
    public IList<GameEventBase> Events => _events.AsReadOnly();
    public abstract string Name { get; }

    public abstract List<GamePlayer> LoadPlayers(int numPlayers);
    public abstract List<GameRoleBase> LoadRoles(int numPlayers);

    protected GameBase(int numPlayers)
    {
        this._roles = this.LoadRoles(numPlayers);
        this._players = this.LoadPlayers(numPlayers);

        LogEvent(new TextEvent($"{Name} started"));

        foreach (GamePlayer player in this._players)
        {
            DealtRoleEvent dealtEvent = new(player, player.InitialRole);

            LogEvent(dealtEvent);
        }

        LogEvent(new TextEvent($"{Name} initialized"));
    }

    protected void LogEvent(GameEventBase @event)
    {
        _events.Add(@event);

        // The player involved should know about this event
        @event.Player?.AddEvent(@event);
    }
}