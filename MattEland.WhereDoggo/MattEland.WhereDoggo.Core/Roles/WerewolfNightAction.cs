using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Roles;

/// <summary>
/// This action applies to any member of team Werewolf that wakes in the night
///
/// Werewolves wake in the night and see other werewolves. In the event that only one werewolf wakes,
/// they may look at a center card.
/// </summary>
public class WerewolfNightAction : RoleNightActionBase
{
    public WerewolfNightAction() : base(new []{RoleTypes.Werewolf, RoleTypes.MysticWolf})
    {
    }

    /// <inheritdoc />
    public override string WakeInstructions => "Werewolves, wake up and see each other. If there is only one wolf, look at a card from the center";

    /// <inheritdoc />
    public override decimal NightActionOrder => 2m;

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        List<GamePlayer> wolves = game.Players.Where(p => p.InitialCard.Team == Teams.Werewolves).ToList();

        switch (wolves.Count)
        {
            case 1:
                HandleLoneWolf(game, player);
                break;

            case > 1:
                // Each wolf knows each other wolf is on team werewolf
                HandleMultipleWolvesWake(game, player, game.Players);
                break;
        }
    }

    private static void HandleLoneWolf(Game game, GamePlayer player)
    {
        game.LogEvent(new OnlyWolfEvent(player));
        foreach (GamePlayer otherPlayer in game.Players.Where(p => p != player))
        {
            MarkTargetAsNonWolf(game, player, otherPlayer);
        }

        IHasCard? cardHolder = player.PickSingleCard(game.CenterSlots);

        if (cardHolder == null)
        {
            game.LogEvent(new SkippedNightActionEvent(player));
        }
        else
        {
            game.LogEvent(new ObservedCenterCardEvent(player, cardHolder));
        }
    }

    private static void MarkTargetAsNonWolf(Game game, GamePlayer player, GamePlayer target)
    {
        foreach (RoleTypes evilRole in game.Roles.Where(r => r.Team == Teams.Werewolves).Select(r => r.RoleType))
        {
            game.LogEvent(new SawNotRoleEvent(player, target, evilRole));
        }
    }

    private static void HandleMultipleWolvesWake(Game game, GamePlayer player, IEnumerable<GamePlayer> wolves)
    {
        foreach (GamePlayer otherPlayer in wolves.Where(otherPlayer => otherPlayer != player))
        {
            if (otherPlayer.InitialCard.Team == Teams.Werewolves)
            {
                game.LogEvent(new SawAsWerewolfEvent(player, otherPlayer));
            }
            else
            {
                MarkTargetAsNonWolf(game, player, otherPlayer);
            }
        }
    }

}