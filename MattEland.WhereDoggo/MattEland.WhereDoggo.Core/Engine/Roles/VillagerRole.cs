namespace MattEland.WhereDoggo.Core.Engine.Roles;

public class VillagerRole : GameRoleBase
{
    public override string ToString() => "Villager";
    public override Teams Team => Teams.Villagers;
    public override RoleTypes RoleType => RoleTypes.Villager;
}