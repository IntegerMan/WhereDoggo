using MattEland.WhereDoggo.Core.Engine.Phases;

namespace MattEland.WhereDoggo.Core.Roles;

public class SentinelNightAction : RoleNightActionBase
{
    public SentinelNightAction() : base(RoleTypes.Sentinel)
    {

    }

    /// <inheritdoc />
    public override string WakeInstructions => "Sentinel, wake up and put a sentinel token on another player's card. That card can no longer be interacted with.";


    /// <inheritdoc />
    public override decimal NightActionOrder => 0.1m;

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