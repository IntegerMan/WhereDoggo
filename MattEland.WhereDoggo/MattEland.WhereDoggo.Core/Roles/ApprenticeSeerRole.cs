namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The apprentice seer can view one card in the middle, much like a Lone Werewolf can.
/// http://onenightultimate.com/?p=53
/// </summary>
public class ApprenticeSeerRole : GameRoleBase
{
    public override RoleTypes RoleType => RoleTypes.ApprenticeSeer;
    public override Teams Team => Teams.Villagers;
}