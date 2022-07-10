namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// This event occurs when a player is observed by another player and now knows their role.
/// </summary>
public class KnowsRoleEvent : GameEventBase
{
    /// <summary>
    /// The player that was observed
    /// </summary>
    public GamePlayer ObservedPlayer { get; }
    
    /// <summary>
    /// The role that the observed player is known to have.
    /// </summary>
    public RoleBase ObservedRole { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KnowsRoleEvent"/> class.
    /// </summary>
    /// <param name="phase">The phase of the game</param>
    /// <param name="observingPlayer">The player observing the other player</param>
    /// <param name="observedPlayer">The player being observed</param>
    public KnowsRoleEvent(GamePhase phase, GamePlayer observingPlayer, GamePlayer observedPlayer) 
        : base(phase, observingPlayer)
    {
        ObservedPlayer = observedPlayer;
        ObservedRole = observedPlayer.CurrentRole;
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