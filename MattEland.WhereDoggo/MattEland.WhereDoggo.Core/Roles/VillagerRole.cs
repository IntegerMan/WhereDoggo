namespace MattEland.WhereDoggo.Core.Roles;

public class VillagerRole : GameRoleBase
{
    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;
    
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Villager;
}