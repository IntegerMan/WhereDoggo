namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The sentinel from One Night Ultimate Werewolf Daybreak
/// The sentinel may optionally place a shield token on a player during the night
/// </summary>
/// <see cref="http://onenightultimate.com/?p=43"/>
public class SentinelRole : GameRoleBase
{
    public override Teams Team => Teams.Villagers;
    public override RoleTypes RoleType => RoleTypes.Sentinel;
}