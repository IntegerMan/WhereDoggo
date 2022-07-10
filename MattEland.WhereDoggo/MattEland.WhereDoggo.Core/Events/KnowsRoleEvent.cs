namespace MattEland.WhereDoggo.Core.Events;

public class KnowsRoleEvent : GameEventBase
{
    public GamePlayer ObservedPlayer { get; }
    public GameRoleBase ObservedRole { get; }

    public KnowsRoleEvent(GamePhase phase, GamePlayer observingPlayer, GamePlayer observedPlayer, GameRoleBase observedRole) 
        : base(phase, observingPlayer)
    {
        ObservedPlayer = observedPlayer;
        ObservedRole = observedRole;
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player} saw that {ObservedPlayer} is a {ObservedRole}";

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, RoleContainerBase target, CardProbabilities probabilities)
    {
        if (target == ObservedPlayer)
        {
            probabilities.MarkAsCertainOfRole(ObservedRole.RoleType);
        }
    }
}