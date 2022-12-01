namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The seer from One Night Ultimate Werewolf.
/// The seer may look at one other player's card, look at 2 cards from the center, or do nothing in her night action.
/// </summary>
/// <href>http://onenightultimate.com/?p=51</href> 
[RoleFor(RoleTypes.Seer)]
public class SeerRole : CardBase
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Seer;

    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;


    /// <inheritdoc />
    public override IEnumerable<NightActionBase> NightActions
    {
        get
        {
            yield return new SeerNightAction();
        }
    }
}