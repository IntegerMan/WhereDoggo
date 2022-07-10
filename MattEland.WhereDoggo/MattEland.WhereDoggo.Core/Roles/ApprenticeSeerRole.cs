namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The sentinel from One Night Ultimate Werewolf Daybreak
/// The apprentice seer can view one card in the middle, much like a Lone Werewolf can.
/// </summary>
/// <seealso cref="http://onenightultimate.com/?p=53"/>
public class ApprenticeSeerRole : GameRoleBase
{
    public override RoleTypes RoleType => RoleTypes.ApprenticeSeer;
    public override Teams Team => Teams.Villagers;
}