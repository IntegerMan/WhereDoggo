namespace MattEland.WhereDoggo.Core.Roles;

public class WerewolfRole : GameRoleBase
{
    public override Teams Team => Teams.Werewolves;

    public override RoleTypes RoleType => RoleTypes.Werewolf;

    public override decimal? NightActionOrder => 2m;
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
            game.LogEvent(new SawNotWerewolfEvent(player, otherPlayer));
        }

        RoleContainerBase? slot = player.Strategies.LoneWolfCenterCardStrategy.SelectSlot(game.CenterSlots);

        if (slot == null)
        {
            throw new InvalidOperationException("A lone werewolf did not pick a center card to look at");
        }

        game.LogEvent(new LoneWolfObservedCenterCardEvent(player, slot, slot.CurrentRole));
    }

    private static void HandleMultipleWolvesWake(Game game, GamePlayer player, List<GamePlayer> wolves)
    {
        foreach (GamePlayer otherPlayer in game.Players.Where(otherPlayer => otherPlayer != player))
        {
            if (otherPlayer.InitialTeam == Teams.Werewolves)
            {
                game.LogEvent(new KnowsRoleEvent(game.CurrentPhase, player, otherPlayer, otherPlayer.CurrentRole));
            }
            else
            {
                game.LogEvent(new SawNotWerewolfEvent(player, otherPlayer));
            }
        }
    }

}