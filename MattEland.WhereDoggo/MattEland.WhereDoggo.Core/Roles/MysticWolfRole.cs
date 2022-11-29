using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The Mystic Wolf is a werewolf that may look at one player's card during the night 
/// </summary>
/// <href>http://onenightultimate.com/?p=37</href>
[RoleFor(RoleTypes.MysticWolf)]
public class MysticWolfRole : WerewolfRole
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.MysticWolf;

    /// <inheritdoc />
    public override Teams Team => Teams.Werewolves;


    /// <inheritdoc />
    public override IEnumerable<NightActionBase> NightActions
    {
        get
        {
            yield return new WerewolfNightAction();
            yield return new MysticWolfNightAction();
        }
    }

}