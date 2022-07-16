namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The seer from One Night Ultimate Werewolf.
/// The seer may look at one other player's card, look at 2 cards from the center, or do nothing in her night action.
/// </summary>
/// <see cref="http://onenightultimate.com/?p=51"/>
[RoleFor(RoleTypes.Seer)]
public class SeerRole : RoleBase
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Seer;

    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override decimal? NightActionOrder => 5m;

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        base.PerformNightAction(game, player);
        
        // TODO: Choose whether we're getting another player or 2 center cards
        
        // TODO: Look at one card from another player
        
        // TODO: Look at two cards from the center
        
        // TODO: Add a skipped night action event
    }
}