namespace MattEland.WhereDoggo.Core.Engine.Events;

public class DealtRoleEvent : GameEventBase
{
    public GameRoleBase Role { get; }

    public DealtRoleEvent(GamePlayer player, GameRoleBase role) : base(player)
    {
        if (player == null) throw new ArgumentNullException(nameof(player));

        Role = role;
    }

    public override string ToString() => $"{Player!.Name} was dealt {Role}";
}