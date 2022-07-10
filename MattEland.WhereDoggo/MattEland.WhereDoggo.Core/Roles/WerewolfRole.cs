namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// Represents a werewolf from One Night Ultimate Werewolf (base game).
/// Werewolves try not to get voted out by the villagers.
/// 
/// Werewolves wake in the night and see other werewolves. In the event that only one werewolf wakes,
/// they may look at a center card.
/// </summary>
/// <href>http://onenightultimate.com/?p=33</href>
public class WerewolfRole : RoleBase
{
    /// <inheritdoc />
    public override Teams Team => Teams.Werewolves;

    /// <inheritdoc />
    public override RoleTypes RoleType => RoleTypes.Werewolf;

    /// <inheritdoc />
    public override decimal? NightActionOrder => 2m;

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        List<GamePlayer> wolves = game.Players.Where(p => p.InitialTeam == Teams.Werewolves).ToList();

        switch (wolves.Count)
        {
            case 1:
                HandleLoneWolf(game, player);
                break;

            case > 1:
                // Each wolf knows each other wolf is on team werewolf
                HandleMultipleWolvesWake(game, player, wolves);
                break;
        }
    }

    private static void HandleLoneWolf(Game game, GamePlayer player)
    {
        game.LogEvent(new OnlyWolfEvent(player));
        foreach (GamePlayer otherPlayer in game.Players.Where(p => p != player))
        {
            game.LogEvent(new SawNotRoleEvent(player, otherPlayer, RoleTypes.Werewolf));
        }

        RoleContainerBase? slot = player.Strategies.PickSingleCardStrategy.SelectCard(game.CenterSlots);

        if (slot == null)
        {
            game.LogEvent(new SkippedNightActionEvent(player));
        }
        else
        {
            game.LogEvent(new ObservedCenterCardEvent(player, slot));
        }
    }

    private static void HandleMultipleWolvesWake(Game game, GamePlayer player, IEnumerable<GamePlayer> wolves)
    {
        foreach (GamePlayer otherPlayer in wolves.Where(otherPlayer => otherPlayer != player))
        {
            if (otherPlayer.InitialTeam == Teams.Werewolves)
            {
                game.LogEvent(new KnowsRoleEvent(game.CurrentPhase, player, otherPlayer));
            }
            else
            {
                game.LogEvent(new SawNotRoleEvent(player, otherPlayer, RoleTypes.Werewolf));
            }
        }
    }

}