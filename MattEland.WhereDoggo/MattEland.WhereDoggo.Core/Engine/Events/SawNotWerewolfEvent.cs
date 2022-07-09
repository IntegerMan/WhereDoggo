namespace MattEland.WhereDoggo.Core.Engine.Events;

public class SawNotWerewolfEvent : GameEventBase
{
    public RoleContainerBase Target { get; }

    public SawNotWerewolfEvent(GamePlayer observer, RoleContainerBase target) 
        : base(GamePhase.Night, observer)
    {
        Target = target;
    }

    public override string ToString() => $"{Player} saw that {Target} is not a Doggo";

    public override void UpdatePlayerPerceptions(GamePlayer observer, RoleContainerBase target, ContainerRoleProbabilities probabilities)
    {
        if (target == Target)
        {
            probabilities.MarkAsCannotBeRole(RoleTypes.Werewolf);
        }
    }
}