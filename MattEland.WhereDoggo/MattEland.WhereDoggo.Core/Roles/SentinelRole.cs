namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The sentinel from One Night Ultimate Werewolf Daybreak
/// The sentinel may optionally place a shield token on a player during the night
/// </summary>
/// <see cref="http://onenightultimate.com/?p=43"/>
public class SentinelRole : GameRoleBase
{
    public override Teams Team => Teams.Villagers;
    public override RoleTypes RoleType => RoleTypes.Sentinel;

    public override decimal? NightActionOrder => 0.1m;

    public override void PerformNightAction(Game game, GamePlayer player)
    {
        // Sentinels may choose to skip placing their token
        if (player.Strategies.SentinelTokenPlacementStrategy.SelectSlot(game.Players) is GamePlayer target)
        {
            if (target.InitialRole.RoleType == RoleTypes.Sentinel)
            {
                throw new InvalidOperationException($"{player} attempted to place a sentinel token on themselves");
            }

            target.HasSentinelToken = true;
            game.LogEvent(new SentinelTokenPlacedEvent(player, target));
            game.LogEvent(new SentinelTokenObservedEvent(player, target, game.CurrentPhase));
        }
        else
        {
            game.LogEvent(new SentinelSkippedTokenPlacementEvent(player));
        }
    }
}