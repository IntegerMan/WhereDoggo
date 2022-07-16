namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The Mystic Wolf is a werewolf that may look at one player's card during the night 
/// </summary>
/// <href>http://onenightultimate.com/?p=37</href>
[RoleFor(RoleTypes.MysticWolf)]
public class MysticWolfRole : RoleBase
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.MysticWolf;

    /// <inheritdoc />
    public override Teams Team => Teams.Werewolves;
}