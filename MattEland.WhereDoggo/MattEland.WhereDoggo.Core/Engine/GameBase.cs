namespace MattEland.WhereDoggo.Core.Engine;

public abstract class GameBase
{
    private readonly List<GamePlayer> _players;
    private readonly List<GameRoleBase> _roles;

    public IList<GamePlayer> Players => _players.AsReadOnly();
    public IList<GameRoleBase> Roles => _roles.AsReadOnly();
    public abstract string Name { get; }

    public abstract List<GamePlayer> LoadPlayers(int numPlayers);
    public abstract List<GameRoleBase> LoadRoles(int numPlayers);

    protected GameBase(int numPlayers)
    {
        this._roles = this.LoadRoles(numPlayers);
        this._players = this.LoadPlayers(numPlayers);
    }
}