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

    public override string ToString() => $"{Player!.Name} was dealt {Role}";

    public override void UpdatePlayerPerceptions(GamePlayer observer, RoleContainerBase target, ContainerRoleProbabilities probabilities)
    {
        if (observer == target)
        {
            probabilities.MarkAsCertainOfRole(Role.RoleType);
        }
    }
}