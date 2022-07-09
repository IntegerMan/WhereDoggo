namespace MattEland.WhereDoggo.Core.Events;

public class InsomniacSawOwnCardEvent : GameEventBase
{
    public InsomniacSawOwnCardEvent(GamePlayer player) : base(GamePhase.Night, player)
    {
        FinalRole = player.CurrentRole;
    }

    public GameRoleBase FinalRole { get; }

    public override void UpdatePlayerPerceptions(GamePlayer observer, RoleContainerBase target, ContainerRoleProbabilities probabilities)
    {
        if (observer == target)
        {
            probabilities.MarkAsCertainOfRole(target.CurrentRole.RoleType);
        }
    }

    public override string ToString() =>
        FinalRole.RoleType == Player!.InitialRole.RoleType 
            ? $"{Player} woke up and saw that they were still {FinalRole.RoleType}" 
            : $"{Player} woke up and saw that they had become a {FinalRole.RoleType}";
}