namespace MattEland.WhereDoggo.Core.Engine.Events;

public class LookAtCenterCardEvent : GameEventBase
{
    public GameRoleBase ObservedRole { get; }

    public LookAtCenterCardEvent(GamePlayer player, GameRoleBase observedRole) : base(player)
    {
        if (player == null) throw new ArgumentNullException(nameof(player));

        ObservedRole = observedRole;
    }

    public override string ToString() => $"{Player!.Name} saw {ObservedRole}";
}