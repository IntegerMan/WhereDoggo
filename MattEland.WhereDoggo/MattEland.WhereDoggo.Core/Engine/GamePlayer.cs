namespace MattEland.WhereDoggo.Core.Engine;

public class GamePlayer
{
    private readonly List<GameEventBase> _events = new();

    public GamePlayer(string name, GameRoleBase initialRole)
    {
        this.Name = name;
        this.InitialRole = initialRole;
        this.CurrentRole = initialRole;
    }

    public string Name { get; }

    public GameRoleBase InitialRole { get; }

    public void AddEvent(GameEventBase eventBase)
    {
        this._events.Add(eventBase);
    }

    public IList<GameEventBase> Events => _events.AsReadOnly();
    public GameRoleBase CurrentRole { get; set; }

    public override string ToString() => Name;
}