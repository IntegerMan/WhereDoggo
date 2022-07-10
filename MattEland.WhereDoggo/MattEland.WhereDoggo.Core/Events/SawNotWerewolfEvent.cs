namespace MattEland.WhereDoggo.Core.Events;

public class SawNotWerewolfEvent : GameEventBase
{
    public RoleContainerBase Target { get; }

    public SawNotWerewolfEvent(GamePlayer observer, RoleContainerBase target) 
        : base(GamePhase.Night, observer)
    {
        Target = target;
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} saw that {Target} is not a werewolf";

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, RoleContainerBase target, CardProbabilities probabilities)
    {
        if (target == Target)
        {
            probabilities.MarkAsCannotBeRole(RoleTypes.Werewolf);
        }
    }
}