namespace MattEland.WhereDoggo.Core.Engine;

public class GamePlayer : RoleContainerBase
{
    private readonly List<GameEventBase> _events = new();

    public GamePlayer(string name, GameRoleBase initialRole) : base(name, initialRole)
    {
    }

    public void AddEvent(GameEventBase eventBase)
    {
        this._events.Add(eventBase);
    }

    public IList<GameEventBase> Events => _events.AsReadOnly();
}