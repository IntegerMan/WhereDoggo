namespace MattEland.WhereDoggo.Core.Events;

/// <summary>
/// Occurs at the beginning of the game and lets the player know what their role is.
/// </summary>
public class DealtRoleEvent : GameEventBase
{
    /// <summary>
    /// The role the player was dealt
    /// </summary>
    public RoleBase Role { get; }

    /// <summary>
    /// Instantiates a new instance of the <see cref="DealtRoleEvent"/> class.
    /// </summary>
    /// <param name="player">The player receiving the role</param>
    /// <param name="role">The role receivedS</param>
    public DealtRoleEvent(GamePlayer player, RoleBase role) : base(GamePhase.Setup, player)
    {
        Role = role;
    }

    /// <inheritdoc />
    public override string ToString() => $"{Player!.Name} was dealt {Role}";
    

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, CardContainer target, CardProbabilities probabilities)
    {
        if (observer == target)
        {
            probabilities.MarkAsCertainOfRole(Role.RoleType);
        }
    }
}