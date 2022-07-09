namespace MattEland.WhereDoggo.Core.Roles;

public class WerewolfRole : GameRoleBase
{
    public override Teams Team => Teams.Werewolves;

    public override RoleTypes RoleType => RoleTypes.Werewolf;
}