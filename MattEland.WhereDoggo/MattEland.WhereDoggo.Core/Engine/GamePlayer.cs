namespace MattEland.WhereDoggo.Core.Engine;

public class GamePlayer
{
    public GamePlayer(string name, GameRoleBase initialRole)
    {
        this.Name = name;
        this.InitialRole = initialRole;
    }

    public string Name { get; }

    public GameRoleBase InitialRole { get; }
}