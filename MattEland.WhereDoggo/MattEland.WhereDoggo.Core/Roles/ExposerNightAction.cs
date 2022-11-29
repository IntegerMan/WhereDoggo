using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Roles;

public class ExposerNightAction : RoleNightActionBase
{
    public ExposerNightAction() : base(RoleTypes.Exposer)
    {

    }

    /// <inheritdoc />
    public override decimal NightActionOrder => 10.2m;

    /// <inheritdoc />
    public override string WakeInstructions => "Exposer, wake up and turn over one center card, leaving its role exposed";

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        int numToExpose = game.Options.ExposerOptions.DetermineCardsToExpose(game.Randomizer);

        for (int i = 0; i < numToExpose; i++)
        {
            IHasCard? holder = player.PickSingleCard(game.CenterSlots.Where(s => !s.CurrentCard.IsRevealed));

            // Exposers may choose to skip exposing things
            if (holder == null)
            {
                game.LogEvent(new SkippedNightActionEvent(player));
                return;
            }

            holder.CurrentCard.IsRevealed = true;
            game.LogEvent(new RevealedRoleEvent(player, holder));
            game.LogEvent(new RevealedRoleObservedEvent(player, holder));
        }
    }
}