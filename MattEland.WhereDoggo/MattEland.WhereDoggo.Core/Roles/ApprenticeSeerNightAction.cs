using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Roles;

public class ApprenticeSeerNightAction : RoleNightActionBase
{
    public ApprenticeSeerNightAction() : base(RoleTypes.ApprenticeSeer)
    {
    }

    /// <inheritdoc />
    public override decimal NightActionOrder => 5.1m;

    /// <inheritdoc />
    public override string WakeInstructions => "Apprentice Seer, choose a card in the middle to view";

    /// <inheritdoc />
    public override void PerformNightAction(Game game, GamePlayer player)
    {
        IHasCard? slot = player.PickSingleCard(game.CenterSlots.Cast<IHasCard>().ToList());

        if (slot == null)
        {
            game.LogEvent(new SkippedNightActionEvent(player));
        }
        else
        {
            game.LogEvent(new ObservedCenterCardEvent(player, slot));
        }
    }

}