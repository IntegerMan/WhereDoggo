﻿namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// The sentinel from One Night Ultimate Werewolf Daybreak
/// The sentinel may optionally place a shield token on a player during the night
/// </summary>
/// <href>http://onenightultimate.com/?p=43</href>
[RoleFor(RoleTypes.Sentinel)]
public class SentinelRole : CardBase
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
        List<IHasCard> otherPlayers = game.Players.Where(p => p != player).Cast<IHasCard>().ToList();
        if (player.PickSingleCard(otherPlayers) is GamePlayer target)
        {
            target.HasSentinelToken = true;
            game.LogEvent(new SentinelTokenPlacedEvent(player, target));
            game.LogEvent(new SentinelTokenObservedEvent(player, target));
        }
        else
        {
            game.LogEvent(new SkippedNightActionEvent(player));
        }
    }
}