using MattEland.WhereDoggo.Core.Engine.Phases;
using MattEland.WhereDoggo.Core.Events.Claims;

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

    /// <inheritdoc />
    public override IEnumerable<ClaimBase> GenerateClaims(GamePlayer player)
    {
        SkippedNightActionEvent? skipped = player.Events.OfType<SkippedNightActionEvent>().FirstOrDefault();
        if (skipped != null)
        {
            yield return new SkippedNightActionClaim(player);
        }
        else
        {
            ObservedCenterCardEvent? saw = player.Events.OfType<ObservedCenterCardEvent>().FirstOrDefault();
            if (saw != null)
            {
                yield return new SawCardClaim(player, saw.Target, saw.ObservedRole);
            }
        }
    }
}