namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The sentinel from One Night Ultimate Werewolf Daybreak
/// The apprentice seer can view one card in the middle, much like a Lone Werewolf can.
/// </summary>
/// <href>http://onenightultimate.com/?p=53</href>
public class ApprenticeSeerRole : GameRoleBase
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.ApprenticeSeer;

    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;
}