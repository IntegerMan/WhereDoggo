namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The Thing is a villager role that may tap the shoulder of the player to their left or right (adjacent).
/// </summary>
/// <href>http://onenightultimate.com/?p=273</href>
[RoleFor(RoleTypes.Thing)]
public class ThingRole : RoleBase
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Thing;

    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override decimal? NightActionOrder => 4.2m;
    
    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        base.PerformNightAction(game, player);
        
        int prevIndex = game.GetPreviousPlayerIndex(player);
        int nextIndex = game.GetNextPlayerIndex(player);
        
        GamePlayer[] options = {game.Players[prevIndex], game.Players[nextIndex]};

        if (player.PickSingleCard(options) is not GamePlayer target)
        {
            game.LogEvent(new SkippedNightActionEvent(player));
        }
        else
        {
            ThingTappedEvent tappedEvent = new(player, target);
            game.LogEvent(tappedEvent);
            target.LogEvent(tappedEvent);
        }
    }
}