using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// Represents a werewolf from One Night Ultimate Werewolf (base game).
/// Werewolves try not to get voted out by the villagers.
/// </summary>
/// <href>http://onenightultimate.com/?p=33</href>
[RoleFor(RoleTypes.Werewolf)]
public class WerewolfRole : CardBase
{
    /// <inheritdoc />
    public override Teams Team => Teams.Werewolves;

    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Werewolf;

    /// <inheritdoc />
    public override IEnumerable<NightActionBase> NightActions
    {
        get
        {
            yield return new WerewolfNightAction();
        }
    }

}