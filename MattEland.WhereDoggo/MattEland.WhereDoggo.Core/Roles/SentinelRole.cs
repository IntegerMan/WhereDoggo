namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The sentinel from One Night Ultimate Werewolf Daybreak
/// The sentinel may optionally place a shield token on a player during the night
/// </summary>
/// <href>http://onenightultimate.com/?p=43</href>
[RoleFor(RoleTypes.Sentinel)]
public class SentinelRole : CardBase
{
    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Sentinel;

    /// <inheritdoc />
    public override IEnumerable<NightActionBase> NightActions
    {
        get
        {
            yield return new SentinelNightAction();
        }
    }
}