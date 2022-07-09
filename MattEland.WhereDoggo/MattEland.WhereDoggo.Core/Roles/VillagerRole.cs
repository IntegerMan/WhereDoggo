namespace MattEland.WhereDoggo.Core.Roles;

public class VillagerRole : GameRoleBase
{
    public override Teams Team => Teams.Villagers;
    public override RoleTypes RoleType => RoleTypes.Villager;
}