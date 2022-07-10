namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The sentinel from One Night Ultimate Werewolf Daybreak
/// The sentinel may optionally place a shield token on a player during the night
/// </summary>
/// <href>http://onenightultimate.com/?p=43</href>
public class SentinelRole : RoleBase
{
    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Sentinel;

    /// <inheritdoc />
    public override decimal? NightActionOrder => 0.1m;

    /// <inheritdoc />
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
            game.LogEvent(new SkippedNightActionEvent(player));
        }
    }
}