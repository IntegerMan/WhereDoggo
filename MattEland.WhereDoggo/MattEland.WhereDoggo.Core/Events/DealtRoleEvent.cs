using MattEland.WhereDoggo.Core.Gamespace;
using MattEland.WhereDoggo.Core.Roles;

namespace MattEland.WhereDoggo.Core.Events;

public class DealtRoleEvent : GameEventBase
{
    public GameRoleBase Role { get; }

    public DealtRoleEvent(GamePlayer player, GameRoleBase role) 
        : base(GamePhase.Setup, player)
    {
        if (player == null) throw new ArgumentNullException(nameof(player));

        Role = role;
    }

    /// <inheritdoc />

    public override string ToString() => $"{Player!.Name} was dealt {Role}";
    

    /// <inheritdoc />
    public override void UpdatePlayerPerceptions(GamePlayer observer, RoleContainerBase target, CardProbabilities probabilities)
    {
        if (observer == target)
        {
            probabilities.MarkAsCertainOfRole(Role.RoleType);
        }
    }
}