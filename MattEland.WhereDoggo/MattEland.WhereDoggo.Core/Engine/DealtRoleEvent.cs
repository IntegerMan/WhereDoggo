namespace MattEland.WhereDoggo.Core.Engine;

public class DealtRoleEvent : GameEventBase
{
    public GameRoleBase Role { get; }

    public DealtRoleEvent(GamePlayer player, GameRoleBase role) : base(player)
    {
        Role = role;
    }

    public override string ToString() => $"{Player.Name} was dealt {Role}";
}