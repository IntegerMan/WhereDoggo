﻿namespace MattEland.WhereDoggo.Core.Roles;


/// <summary>
/// The Exposer role from One Night Ultimate Werewolf Alien expansion.
/// The Exposer may reveal a random number (1-3) of cards from the center
/// </summary>
/// <href>https://one-night.fandom.com/wiki/Exposer</href>
public class ExposerRole : RoleBase
{
    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Exposer;

    /// <inheritdoc />
    public override Teams Team => Teams.Villagers;

    /// <inheritdoc />
    public override decimal? NightActionOrder { get; }

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        base.PerformNightAction(game, player);
    }
}