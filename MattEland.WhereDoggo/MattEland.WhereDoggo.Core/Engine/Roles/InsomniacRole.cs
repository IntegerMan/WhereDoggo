namespace MattEland.WhereDoggo.Core.Engine.Roles;

public class InsomniacRole : GameRoleBase
{
 
    public override string ToString() => "Insomniac";
    public override Teams Team => Teams.Villagers;
    public override RoleTypes RoleType => RoleTypes.Insomniac;
}