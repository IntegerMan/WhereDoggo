namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The Exposer role from One Night Ultimate Werewolf Alien expansion.
/// The Exposer may reveal a random number (1-3) of cards from the center
/// </summary>
/// <href>https://one-night.fandom.com/wiki/Exposer</href>
[RoleFor(RoleTypes.Exposer)]
public class ExposerRole : CardBase
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Exposer;

    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override IEnumerable<NightActionBase> NightActions
    {
        get
        {
            yield return new ExposerNightAction();
        }
    }
}
