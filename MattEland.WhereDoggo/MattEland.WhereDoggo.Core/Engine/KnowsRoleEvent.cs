namespace MattEland.WhereDoggo.Core.Engine;

public class KnowsRoleEvent : GameEventBase
{
    public GamePlayer ObservedPlayer { get; }
    public GameRoleBase ObservedRole { get; }

    public KnowsRoleEvent(GamePlayer observingPlayer, GamePlayer observedPlayer, GameRoleBase observedRole) : base(observingPlayer)
    {
        ObservedPlayer = observedPlayer;
        ObservedRole = observedRole;
    }

    public override string ToString() => $"{Player} saw that {ObservedPlayer} is a {ObservedRole}";
}