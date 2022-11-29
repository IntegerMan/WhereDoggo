using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The insomniac from One Night Ultimate Werewolf.
/// Insomniacs wake near the end of the night and look at their own card to see if their role changed.
/// </summary>
/// <href>http://onenightultimate.com/?p=70</href>
[RoleFor(RoleTypes.Insomniac)]
public class InsomniacRole : CardBase
{
    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Insomniac;

    /// <inheritdoc />
    public override IEnumerable<NightActionBase> NightActions
    {
        get
        {
            yield return new InsomniacNightAction();
        }
    }

}