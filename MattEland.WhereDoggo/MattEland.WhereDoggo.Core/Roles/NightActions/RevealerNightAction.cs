namespace MattEland.WhereDoggo.Core.Roles.NightActions;

public class RevealerNightAction : RoleNightActionBase
{
    public RevealerNightAction() : base(RoleTypes.Revealer)
    {

    }

    /// <inheritdoc />
    public override string WakeInstructions => "Revealer, wake up and flip over another player's card";


    /// <inheritdoc />
    public override decimal NightActionOrder => 10m;

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        IHasCard? target = player.PickSingleCard(game.GetOtherPlayerTargets(player));

        if (target == null)
        {
            game.LogEvent(new SkippedNightActionEvent(player));
        }
        else
        {
            GamePlayer targetPlayer = (GamePlayer)target;

            targetPlayer.CurrentCard.IsRevealed = true;
            game.LogEvent(new RevealedRoleEvent(player, targetPlayer, target.CurrentCard.RoleType));
            game.LogEvent(new RevealedRoleObservedEvent(player, targetPlayer));

            // Only villagers should be revealed
            if (targetPlayer.CurrentCard.Team != Teams.Villagers)
            {
                targetPlayer.CurrentCard.IsRevealed = false;
                game.LogEvent(new RevealerHidEvilRoleEvent(player, targetPlayer));
            }
        }
    }
}