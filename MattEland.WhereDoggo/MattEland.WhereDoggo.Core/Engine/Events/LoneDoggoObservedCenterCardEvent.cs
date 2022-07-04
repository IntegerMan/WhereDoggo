namespace MattEland.WhereDoggo.Core.Engine.Events;

public class LoneDoggoObservedCenterCardEvent : GameEventBase
{
    public RoleContainerBase ObservedSlot { get; }
    public GameRoleBase ObservedRole { get; }

    public LoneDoggoObservedCenterCardEvent(GamePlayer player, RoleContainerBase observedSlot, GameRoleBase observedRole) 
        : base(GamePhase.Night, player)
    {
        if (player == null) throw new ArgumentNullException(nameof(player));

        ObservedSlot = observedSlot;
        ObservedRole = observedRole;
    }

    public override string ToString() => $"{Player} saw {ObservedRole} in {ObservedSlot}";

    public override void UpdatePlayerPerceptions(GamePlayer observer, RoleContainerBase target, ContainerRoleProbabilities probabilities)
    {
        if (target == ObservedSlot)
        {
            probabilities.MarkAsCertainOfRole(ObservedRole);
        }
    }
}