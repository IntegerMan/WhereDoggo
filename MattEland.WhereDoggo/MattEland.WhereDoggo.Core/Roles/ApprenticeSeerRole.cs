using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The sentinel from One Night Ultimate Werewolf Daybreak
/// The apprentice seer can view one card in the middle, much like a Lone Werewolf can.
/// </summary>
/// <href>http://onenightultimate.com/?p=53</href>
[RoleFor(RoleTypes.ApprenticeSeer)]
public class ApprenticeSeerRole : CardBase
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.ApprenticeSeer;

    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override IEnumerable<NightActionBase> NightActions
    {
        get
        {
            yield return new ApprenticeSeerNightAction();
        }
    }
}
