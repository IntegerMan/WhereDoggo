using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Roles;

public class ThingNightAction : RoleNightActionBase
{
    public ThingNightAction() : base(RoleTypes.Thing)
    {

    }

    /// <inheritdoc />
    public override string WakeInstructions => "The Thing, wake up and tap one of your neighboring players";

    /// <inheritdoc />
    public override decimal NightActionOrder => 4.2m;

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        int prevIndex = game.GetPreviousPlayerIndex(player);
        int nextIndex = game.GetNextPlayerIndex(player);

        GamePlayer[] options = { game.Players[prevIndex], game.Players[nextIndex] };

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